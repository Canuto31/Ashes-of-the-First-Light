using TMPro;
using UnityEngine;

public class InventoryCategoryOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetTitle(string title)
    {
        if (_text != null)
            _text.text = title;
    }
}
