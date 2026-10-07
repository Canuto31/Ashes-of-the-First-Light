using System;

[Serializable]
/// <summary>
/// Provides the runtime behavior and data owned by the inventory entry component.
/// </summary>
public class InventoryEntry
{
    public InventoryItem Item;
    public int Quantity;
    public bool Identified;

    public InventoryEntry(InventoryItem item, int quantity = 1, bool identified = false)
    {
        Item = item;
        Quantity = quantity;
        Identified = identified;
    }
}
