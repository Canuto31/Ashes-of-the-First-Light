using TMPro;
using UnityEngine;

public class TutorialOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _optionText;

    public void SetTitle(string title)
    {
        if (_optionText != null)
            _optionText.text = title;
    }
}
