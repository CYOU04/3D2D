using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameViewSwitcher : MonoBehaviour
{
    public enum GameMode
    {
        Mode3D,
        Mode2D
    }

    public GameMode currentMode = GameMode.Mode3D;

    private float orthoSize = 5f;//field of vision 2D
    private float rotateSpeed = 5f;
    private float cameraDistance2D = 10f;
    private float cameraHeight2D = 2f;

    [HideInInspector] public float target2DYRotation = 0f;
    private float current2DYRotation = 0f;

    private float fov3D = 60f;

    public Camera3D camera3DScript;
    private Camera mainCam;
    private Transform camBoom3D;

    public static bool is2DMode = false;

    [Header("Black Screen Transition")]
    [SerializeField] private float fadeToBlackDuration = 0.2f;
    [SerializeField] private float fadeFromBlackDuration = 0.25f;

    private CanvasGroup transitionCanvasGroup;
    private bool isTransitioning;

    void Start()
    {
        mainCam = Camera.main;
        CreateTransitionOverlay();

        if (camera3DScript != null)
        {
            camBoom3D = camera3DScript.transform;
        }
        else
        {
            camera3DScript = GetComponentInChildren<Camera3D>();
            if (camera3DScript != null)
            {
                camBoom3D = camera3DScript.transform;
            }
        }

        ApplyMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isTransitioning && Time.timeScale > 0f)
        {
            StartCoroutine(SwitchModeWithTransition());
        }

        if (currentMode == GameMode.Mode2D && !isTransitioning)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                target2DYRotation += 90f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                target2DYRotation -= 90f;
            }
        }
    }

    private IEnumerator SwitchModeWithTransition()
    {
        isTransitioning = true;

        yield return FadeOverlay(0f, 1f, fadeToBlackDuration);

        currentMode = (currentMode == GameMode.Mode3D) ? GameMode.Mode2D : GameMode.Mode3D;
        ApplyMode();

        // Wait one frame so the new camera mode is rendered behind the black overlay.
        yield return null;
        yield return FadeOverlay(1f, 0f, fadeFromBlackDuration);

        isTransitioning = false;
    }

    private IEnumerator FadeOverlay(float startAlpha, float endAlpha, float duration)
    {
        if (transitionCanvasGroup == null)
        {
            yield break;
        }

        if (duration <= 0f)
        {
            transitionCanvasGroup.alpha = endAlpha;
            yield break;
        }

        float elapsed = 0f;
        transitionCanvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            transitionCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }

        transitionCanvasGroup.alpha = endAlpha;
    }

    private void CreateTransitionOverlay()
    {
        GameObject canvasObject = new GameObject(
            "ViewTransitionCanvas",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster),
            typeof(CanvasGroup)
        );

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;

        transitionCanvasGroup = canvasObject.GetComponent<CanvasGroup>();
        transitionCanvasGroup.alpha = 0f;
        transitionCanvasGroup.interactable = false;
        transitionCanvasGroup.blocksRaycasts = false;

        GameObject overlayObject = new GameObject("BlackOverlay", typeof(RectTransform), typeof(Image));
        overlayObject.transform.SetParent(canvasObject.transform, false);

        RectTransform overlayRect = overlayObject.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;

        Image overlayImage = overlayObject.GetComponent<Image>();
        overlayImage.color = Color.black;
        overlayImage.raycastTarget = false;
    }

    void LateUpdate()
    {
        if (mainCam == null)
        {
            return;
        }

        if (currentMode == GameMode.Mode2D)
        {
            is2DMode = true;

            current2DYRotation = Mathf.LerpAngle(current2DYRotation, target2DYRotation, Time.deltaTime * rotateSpeed);
            Quaternion camRotation = Quaternion.Euler(0f, current2DYRotation, 0f);

            Vector3 directionOffset = camRotation * new Vector3(0f, 0f, -cameraDistance2D);
            Vector3 targetCamPosition = transform.position + directionOffset;
            targetCamPosition.y += cameraHeight2D;

            mainCam.transform.position = targetCamPosition;
            mainCam.transform.rotation = camRotation;
        }
    }

    void ApplyMode()
    {
        if (mainCam == null)
        {
            return;
        }

        if (currentMode == GameMode.Mode3D)
        {
            is2DMode = false;

            mainCam.orthographic = false;//perspective
            mainCam.fieldOfView = fov3D;

            if (camBoom3D != null)
            {
                camBoom3D.position = transform.position;

                mainCam.transform.SetParent(camBoom3D);
                mainCam.transform.localPosition = new Vector3(0f, 2f, -7f);
                mainCam.transform.localRotation = Quaternion.identity;
            }

            if (camera3DScript != null)
            {
                camera3DScript.enabled = true;                
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            mainCam.orthographic = true;//orthographic
            mainCam.orthographicSize = orthoSize;

            mainCam.transform.SetParent(null);

            if (camera3DScript != null)
            {
                camera3DScript.enabled = false;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float currentY = transform.eulerAngles.y;
            target2DYRotation = Mathf.Round(currentY / 90f) * 90f;
            current2DYRotation = target2DYRotation;

            transform.rotation = Quaternion.Euler(0f, target2DYRotation, 0f);
        }
    }
}
