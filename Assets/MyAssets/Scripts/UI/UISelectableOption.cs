using TMPro;
using UnityEngine;

public class UISelectableOption : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _optionText;

    [SerializeField] private Color _normalColor = Color.gray;
    [SerializeField] private Color _selectedColor = Color.white;

    [Header("Settings")] 
    [SerializeField] private float _normalSize = 28f;
    [SerializeField] private float _selectedSize = 34f;

    public void SetSelected(bool selected)
    {
        _optionText.color = selected ? _selectedColor : _normalColor;
        
        _optionText.alpha = selected ? _selectedSize : _normalSize;
    }
}
