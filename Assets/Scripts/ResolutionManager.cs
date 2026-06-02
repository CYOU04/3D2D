using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager Instance { get; private set; }

    public bool isFullScreen = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            ApplyResolution();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleResolution()
    {
        isFullScreen = !isFullScreen;
        ApplyResolution();
    }
    private void ApplyResolution()
    {
        if (isFullScreen)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }
}
