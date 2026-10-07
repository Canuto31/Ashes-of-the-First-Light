using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the pickup item component.
/// </summary>
public class PickupItem : MonoBehaviour, IInteractable
{

    #region Fields and Configuration

    [SerializeField] private InventoryItem _item;
    [SerializeField] private int _quantity = 1;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Returns the prompt text presented for this interaction.
    /// </summary>
    public string GetInteractionText()
    {
        return _item != null ? "Pick up " + _item.itemName : "Pick up item";
    }

    /// <summary>
    /// Executes this object's response to a valid player interaction.
    /// </summary>
    public void Interact()
    {
        if (_item == null || PlayerInventory.Instance == null)
            return;

        PlayerInventory.Instance.AddItem(_item, _quantity);
        UI_Interaction.Instance?.ShowTextTimed(_item.itemName + " acquired", 1f);

        Destroy(gameObject);
    }

    #endregion
}
