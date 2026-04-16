using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Interaction : MonoBehaviour
{
    public static UI_Interaction Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake() {
        Instance = this;
        Hide();
    }

    public void ShowText(string message)
    {
        panel.SetActive(true);
        text.text = message;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
