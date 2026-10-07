using TMPro;
using UnityEngine;

/// <summary>
/// Presents and updates inventory category option user-interface data.
/// </summary>
public class InventoryCategoryOptionUI : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private TextMeshProUGUI _text;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Updates title for this component.
    /// </summary>
    public void SetTitle(string title)
    {
        if (_text != null)
            _text.text = title;
    }

    #endregion
}
