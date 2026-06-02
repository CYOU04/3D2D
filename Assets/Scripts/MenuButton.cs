using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
    public void HowtoPlay()
    {
        SceneManager.LoadScene("Guide");
    }
}
