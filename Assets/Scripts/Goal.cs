using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private Vector3 rotateSpeed = new Vector3(0, 100f, 0);
    private float floatAmplitude = 1f;
    private float floatFrequency = 2f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.deltaTime * floatFrequency) * floatAmplitude;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Result");
        }
    }
}
