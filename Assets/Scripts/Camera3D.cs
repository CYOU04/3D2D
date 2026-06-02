using UnityEngine;

public class Camera3D : MonoBehaviour
{
    public float mouseSensitivity = 50f;

    //get player
    public Transform playerTransform;

    private float minPitch = -30f;
    private float maxPitch = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    void Start()
    {
        Application.targetFrameRate = 60;

        //hide and lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible  = false;

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 50f);

        if (playerTransform == null)
        {
            playerTransform = transform.parent;
        }

        if (playerTransform != null)
        {
            rotationY = playerTransform.eulerAngles.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()   //execute at the end of each frame
    {
        if (playerTransform == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationY += mouseX;
        rotationX -= mouseY;

        //limit vertical angle
        rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);

        //the camera is responsible for looking up and down.
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        //the player is responsible for turning around
        playerTransform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    public void SetSensitivity(float newValue)
    {
        mouseSensitivity = newValue;
    }
}
