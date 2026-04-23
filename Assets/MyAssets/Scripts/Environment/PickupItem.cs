using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItem _item;

    public string GetInteractionText()
    {
        return "Pick up " + _item.itemName;
    }

    public void Interact()
    {
        PlayerInventory.Instance.AddItem(_item);

        UI_Interaction.Instance.ShowTextTimed(_item.itemName + " acquired", 1f);

        Destroy(gameObject);
    }
}
