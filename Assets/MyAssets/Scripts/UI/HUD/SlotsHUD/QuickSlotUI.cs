using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _lockIcon;
    [SerializeField] private TextMeshProUGUI _keyLabel;

    public void Setkey(string key)
    {
        _keyLabel.text = key;
    }

    public void SetState(QuickSlotState state)
    {
        switch (state)
        {
            case QuickSlotState.Locked:
                _lockIcon.gameObject.SetActive(true);
                _itemIcon.gameObject.SetActive(false);

                break;
            case QuickSlotState.Unlocked:
                _lockIcon.gameObject.SetActive(false);
                _itemIcon.gameObject.SetActive(true);

                break;
            case QuickSlotState.Equipped:
                _lockIcon.gameObject.SetActive(false);
                _itemIcon.gameObject.SetActive(true);

                break;
        }
    }
}
