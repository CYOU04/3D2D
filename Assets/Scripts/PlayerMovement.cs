using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Moving
    private float moveSpeed = 6f;
    private float jumpHeight = 2f;
    private float gravity = -9.8f;

    public PlayerGroundCheck playerGroundCheck;

    private GameViewSwitcher viewSwitcher;

    private CharacterController characterController;
    private Vector3 velocity;
    public bool isGrounded;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        viewSwitcher = GetComponent<GameViewSwitcher>();
    }

    void Update()
    {
        if (playerGroundCheck != null)
        {
            isGrounded = playerGroundCheck.isGrounded;
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Vector3.zero;

        if (viewSwitcher != null && viewSwitcher.currentMode == GameViewSwitcher.GameMode.Mode2D)
        {
            Vector3 cam2DRight = Camera.main.transform.right;
            cam2DRight.y = 0f;
            cam2DRight.Normalize();

            move = cam2DRight * x;

            if (x > 0.1f)
            {
                transform.forward = cam2DRight;
            }
            else if (x < -0.1f)
            {
                transform.forward = -cam2DRight;
            }
        }
        else
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            move = camRight * x + camForward * z;
        }
        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("jump");
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
