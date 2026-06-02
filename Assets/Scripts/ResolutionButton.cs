using UnityEngine;
using TMPro;

public class ResolutionButton : MonoBehaviour
{
    private TMP_Text resolutionText;
    void Start()
    {
        resolutionText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonText();
    }
    public void OnButtonClick()
    {
        if (ResolutionManager.Instance != null)
        {
            ResolutionManager.Instance.ToggleResolution();
            UpdateButtonText();
        }
    }
    public void UpdateButtonText()
    {
        if (ResolutionManager.Instance == null || resolutionText == null)
            return;

        if (ResolutionManager.Instance.isFullScreen)
        {
            resolutionText.text = "Full Screen";
        }
        else
        {
            resolutionText.text = "Window";
        }
    }

    private void OnEnable()
    {
        UpdateButtonText();
    }
}
