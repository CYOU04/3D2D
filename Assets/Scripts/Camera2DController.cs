using UnityEngine;

public class Camera2DController : MonoBehaviour
{
    private float rotateSpeed = 5f;
    private float targetYRotation = 0f;
    void Start()
    {
        targetYRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetYRotation += 90f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetYRotation -= 90f;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }
}
