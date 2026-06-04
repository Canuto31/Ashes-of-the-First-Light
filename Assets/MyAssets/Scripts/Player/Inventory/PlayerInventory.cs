using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private List<InventoryEntry> _items = new();

    private void Awake() {
        Instance = this;
    }

    public void AddItem(InventoryItem item, int quantity = 1)
    {
        InventoryEntry entry = GetEntry(item);

        if (entry != null)
        {
            entry.Quantity += quantity;
        }
        else
        {
            bool identified = item.category != InventoryCategory.Consumable;
            
            _items.Add(new InventoryEntry(item, quantity, identified));
        }

        Debug.Log($"Item collected: {item.itemName} x{quantity}");
    }

    public bool RemoveItem(InventoryItem item, int quantity = 1)
    {
        InventoryEntry entry = GetEntry(item);

        if (entry == null)
            return false;

        if (entry.Quantity < quantity)
            return false;
        
        entry.Quantity -= quantity;
        
        if (entry.Quantity <= 0)
            _items.Remove(entry);

        return true;
    }

    public bool HasItem(InventoryItem item)
    {
        return GetEntry(item) != null;
    }

    public InventoryEntry GetEntry(InventoryItem item)
    {
        return _items.Find(entry => entry.Item == item);
    }

    public List<InventoryEntry> GetItems()
    {
        return _items;
    }

    public List<InventoryEntry> GetItemsByCategory(InventoryCategory category)
    {
        if (category == InventoryCategory.All)
            return new List<InventoryEntry>(_items);
        
        return _items.FindAll(entry => entry.Item.category == category);
    }

    public void IdentifyItem(InventoryItem item)
    {
        InventoryEntry entry = GetEntry(item);
        
        if (entry != null)
            entry.Identified = true;
    }
}
