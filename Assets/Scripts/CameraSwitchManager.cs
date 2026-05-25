using UnityEngine;

public enum CameraMode
{
    CameraMode3D,
    CameraMode2D
}
public class CameraSwitchManager : MonoBehaviour
{
    public GameObject Camera3D;
    public GameObject Camera2D;

    public CameraMode currentMode = CameraMode.CameraMode3D;
    void Start()
    {
        ApplyCameraMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CameraModeSwitch();
        }
    }
    void CameraModeSwitch()
    {
        if (currentMode == CameraMode.CameraMode3D)
        {
            currentMode = CameraMode.CameraMode2D;
        }
        else
        {
            currentMode = CameraMode.CameraMode3D;
        }

        ApplyCameraMode();
    }
    void ApplyCameraMode()
    {
        if (currentMode == CameraMode.CameraMode3D)
        {
            Camera3D.SetActive(true);
            Camera2D.SetActive(false);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }
        else
        {
            Camera3D.SetActive(false);
            Camera2D.SetActive(true);
        }
    }
}
