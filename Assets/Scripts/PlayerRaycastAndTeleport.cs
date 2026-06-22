using UnityEngine;

public class PlayerRaycastAndTeleport : MonoBehaviour
{
    public Transform checkPoint;
    private float rayDistance = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Vector3 checkBoxHalfExtents = new Vector3(0.5f, 0.05f, 0.5f);

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

        Vector3 leftDirection = -transform.right;
        Vector3 rightDirection = transform.right;
        Quaternion boxRotation = transform.rotation;

        if (Physics.BoxCast(origin, checkBoxHalfExtents, leftDirection, out RaycastHit leftHit,
            boxRotation, rayDistance, targetLayer, QueryTriggerInteraction.Ignore))
        {
            TeleportPlayer(leftHit.collider);
        }
        if (Physics.BoxCast(origin, checkBoxHalfExtents, rightDirection, out RaycastHit rightHit,
            boxRotation, rayDistance, targetLayer, QueryTriggerInteraction.Ignore))
        {
            TeleportPlayer(rightHit.collider);
        }

        Debug.DrawRay(origin, rightDirection * rayDistance, Color.red);
        Debug.DrawRay(origin, leftDirection * rayDistance, Color.blue);
    }
    void TeleportPlayer(Collider rayCollider)
    {
        if (Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
        {
            TeleportPlayerZ(rayCollider);
        }
        else
        {
            TeleportPlayerX(rayCollider);
        }
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
        transform.position = targetPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Matrix4x4 previousMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(checkPoint.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, checkBoxHalfExtents * 2f);
        Gizmos.matrix = previousMatrix;
    }
}