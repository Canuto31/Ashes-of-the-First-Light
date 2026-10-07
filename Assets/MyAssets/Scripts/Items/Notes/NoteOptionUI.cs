using TMPro;
using UnityEngine;

/// <summary>
/// Presents and updates note option user-interface data.
/// </summary>
public class NoteOptionUI : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private TextMeshProUGUI _optionText;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Updates title for this component.
    /// </summary>
    public void SetTitle(string title)
    {
        if (_optionText != null)
            _optionText.text = title;
    }

    #endregion
}
