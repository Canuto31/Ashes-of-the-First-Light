using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private HashSet<InventoryItem> _items = new HashSet<InventoryItem>();

    private void Awake() {
        Instance = this;
    }

    public void AddItem(InventoryItem item) {
        _items.Add(item);
        Debug.Log("Item collected: " + item.itemName);
    }

    public bool HasItem(InventoryItem item) {
        return _items.Contains(item);
    }
}
