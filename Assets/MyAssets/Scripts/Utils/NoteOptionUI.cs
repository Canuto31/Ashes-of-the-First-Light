using TMPro;
using UnityEngine;

public class NoteOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _optionText;

    public void SetTitle(string title)
    {
        _optionText.text = title;
    }
}
