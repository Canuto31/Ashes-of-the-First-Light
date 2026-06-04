using TMPro;
using UnityEngine;

public class InventoryCategoryOptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetTitle(string title)
    {
        _text.text = title;
    }
}
