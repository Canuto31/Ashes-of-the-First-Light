using TMPro;
using UnityEngine;

/// <summary>
/// Presents and updates tutorial option user-interface data.
/// </summary>
public class TutorialOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _optionText;

    public void SetTitle(string title)
    {
        if (_optionText != null)
            _optionText.text = title;
    }
}
