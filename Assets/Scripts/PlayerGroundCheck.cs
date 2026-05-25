using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
public bool isGrounded { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.IsChildOf(transform.root))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.IsChildOf(transform.root))
        {
            isGrounded = false;
        }
    }
}
