using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player inventory component.
/// </summary>
public class PlayerInventory : MonoBehaviour
{

    #region Fields and Configuration

    public static PlayerInventory Instance;

    private readonly List<InventoryEntry> _items = new();


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Adds item for this component.
    /// </summary>
    public void AddItem(InventoryItem item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return;

        InventoryEntry entry = GetEntry(item);

        if (entry != null)
            entry.Quantity += quantity;
        else
            AddNewEntry(item, quantity);

        Debug.Log($"Item collected: {item.itemName} x{quantity}");
    }

    /// <summary>
    /// Adds new entry for this component.
    /// </summary>
    private void AddNewEntry(InventoryItem item, int quantity)
    {
        bool identified = item.category != InventoryCategory.Consumable;
        _items.Add(new InventoryEntry(item, quantity, identified));
    }

    /// <summary>
    /// Removes item for this component.
    /// </summary>
    public bool RemoveItem(InventoryItem item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        InventoryEntry entry = GetEntry(item);

        if (entry == null || entry.Quantity < quantity)
            return false;
        
        entry.Quantity -= quantity;

        if (entry.Quantity <= 0)
            _items.Remove(entry);

        return true;
    }

    /// <summary>
    /// Returns whether item is currently true.
    /// </summary>
    public bool HasItem(InventoryItem item)
    {
        return GetEntry(item) != null;
    }

    /// <summary>
    /// Returns the current entry for this component.
    /// </summary>
    public InventoryEntry GetEntry(InventoryItem item)
    {
        return _items.Find(entry => entry.Item == item);
    }

    /// <summary>
    /// Returns the current items for this component.
    /// </summary>
    public List<InventoryEntry> GetItems()
    {
        return _items;
    }

    /// <summary>
    /// Returns the current items by category for this component.
    /// </summary>
    public List<InventoryEntry> GetItemsByCategory(InventoryCategory category)
    {
        if (category == InventoryCategory.All)
            return new List<InventoryEntry>(_items);
        
        return _items.FindAll(entry => entry.Item.category == category);
    }

    /// <summary>
    /// Executes the identify item operation for this component.
    /// </summary>
    public void IdentifyItem(InventoryItem item)
    {
        InventoryEntry entry = GetEntry(item);
        
        if (entry != null)
            entry.Identified = true;
    }

    #endregion
}
