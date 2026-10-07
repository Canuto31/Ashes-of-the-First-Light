using TMPro;
using UnityEngine;

/// <summary>
/// Presents and updates inventory category option user-interface data.
/// </summary>
public class InventoryCategoryOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetTitle(string title)
    {
        if (_text != null)
            _text.text = title;
    }
}
