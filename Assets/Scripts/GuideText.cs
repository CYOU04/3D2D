using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private float typeSpeed = 0.01f;

    private string fullText = "Objective:\nCollect the gold coins to win!\n\nControls:\nPress [ R ] to toggle between 3D and 2D modes.\n\nIn 3D Mode:\n[ W ][ A ][ S ][ D ] — Move\n[ Mouse ] — Look around\n[ Space ] — Jump\n\nIn 2D Mode:\n[ A ][ D ] — Move Left / Right\n[ Q ][ E ] — Change Camera Angle\n[ Space ] — Jump";

    void Start()
    {
        uiText.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}