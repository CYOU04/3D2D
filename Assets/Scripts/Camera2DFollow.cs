using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
