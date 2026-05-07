using TMPro;
using UnityEngine;

public class UISelectableOption : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _selectorArrow;
    [SerializeField] private TextMeshProUGUI _optionText;

    [Header("Settings")] 
    [SerializeField] private float _selectedScale = 1.1f;

    public void SetSelected(bool selected)
    {
        _selectorArrow.SetActive(selected);
        
        _optionText.alpha = selected ? 1f : 0.5f;
        
        transform.localScale = selected ? Vector3.one * _selectedScale : Vector3.one;
    }
}
