using UnityEngine;

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

    void Start()
    {
        mainCam = Camera.main;

        if (camera3DScript != null)
        {
            camBoom3D = camera3DScript.transform;
        }
        else
        {
            camera3DScript = GetComponentInChildren<Camera3D>();
            if (camera3DScript != null)
                camBoom3D = camera3DScript.transform;
        }

        ApplyMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentMode = (currentMode == GameMode.Mode3D) ? GameMode.Mode2D : GameMode.Mode3D;
            ApplyMode();
        }

        if (currentMode == GameMode.Mode2D)
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

            mainCam.orthographic = false;
            mainCam.fieldOfView = fov3D;

            if (camBoom3D != null)
            {
                camBoom3D.position = transform.position;

                mainCam.transform.SetParent(camBoom3D);
                mainCam.transform.localPosition = new Vector3(0f, 2f, -7f);
                mainCam.transform.localRotation = Quaternion.identity;
            }

            if (camera3DScript != null)
                camera3DScript.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            mainCam.orthographic = true;
            mainCam.orthographicSize = orthoSize;

            mainCam.transform.SetParent(null);

            if (camera3DScript != null)
                camera3DScript.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float currentY = transform.eulerAngles.y;
            target2DYRotation = Mathf.Round(currentY / 90f) * 90f;
            current2DYRotation = target2DYRotation;

            transform.rotation = Quaternion.Euler(0f, target2DYRotation, 0f);
        }
    }
}