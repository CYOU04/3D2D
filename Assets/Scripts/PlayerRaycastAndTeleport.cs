using UnityEngine;

public class PlayerRaycastAndTeleport : MonoBehaviour
{
    public Transform checkPoint;
    private float rayDistance = 10f;
    [SerializeField] private LayerMask targetLayer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameViewSwitcher.is2DMode == true)
        {
            Detect();
        }
    }
    void Detect()
    {
        if (checkPoint == null)
        {
            return;
        }

        Vector3 origin = checkPoint.position;

        RaycastHit leftRay;
        RaycastHit rightRay;

        //float facingDirection = Mathf.Sign(transform.lossyScale.x);
        Vector3 leftDirection = -transform.right;
        Vector3 rightDirection = transform.right;

        if (Physics.Raycast(origin, leftDirection, out leftRay, rayDistance, targetLayer))
        {
            if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
            {
                TeleportPlayerZ(leftRay.collider);
            }
            else
            {
                TeleportPlayerX(leftRay.collider);
            }
        }
        if (Physics.Raycast(origin, rightDirection, out rightRay, rayDistance, targetLayer))
        {
            if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
            {
                TeleportPlayerZ(rightRay.collider);
            }
            else
            {
                TeleportPlayerX(rightRay.collider);
            }
        }

        Debug.DrawRay(origin, rightDirection * rayDistance, Color.red);
        Debug.DrawRay(origin, leftDirection * rayDistance, Color.blue);
    }
    void TeleportPlayerZ(Collider cubeCollider)
    {
        float targetZ = cubeCollider.transform.position.z;

        if (Mathf.Approximately(transform.position.z, targetZ))
        {
            return;
        }

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, targetZ);

        Teleport(newPosition);
    }
    void TeleportPlayerX(Collider cubeCollider)
    {
        float targetX = cubeCollider.transform.position.x;

        if (Mathf.Approximately(transform.position.x, targetX))
        {
            return;
        }

        Vector3 newPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        Teleport(newPosition);
    }
    void Teleport(Vector3 targetPosition)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.position = targetPosition;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            transform.position = targetPosition;
        }
    }
}