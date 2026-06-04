using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItem _item;
    [SerializeField] private int _quantity = 1;

    public string GetInteractionText()
    {
        return "Pick up " + _item.itemName;
    }

    public void Interact()
    {
        PlayerInventory.Instance.AddItem(_item, _quantity);

        UI_Interaction.Instance.ShowTextTimed(_item.itemName + " acquired", 1f);

        Destroy(gameObject);
    }
}
