using TMPro;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the uiselectable option component.
/// </summary>
public class UISelectableOption : MonoBehaviour
{

    #region Fields and Configuration

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _optionText;

    [SerializeField] private Color _normalColor = Color.gray;
    [SerializeField] private Color _selectedColor = Color.white;

    [Header("Settings")] 
    [SerializeField] private float _normalSize = 28f;
    [SerializeField] private float _selectedSize = 34f;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Updates selected for this component.
    /// </summary>
    public void SetSelected(bool selected)
    {
        if (_optionText == null)
            return;

        _optionText.color = selected ? _selectedColor : _normalColor;
        _optionText.fontSize = selected ? _selectedSize : _normalSize;
    }

    #endregion
}
