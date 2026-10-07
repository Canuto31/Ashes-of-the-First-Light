using UnityEngine;

[CreateAssetMenu(menuName = "Game/Inventory Item")]
/// <summary>
/// Provides the runtime behavior and data owned by the inventory item component.
/// </summary>
public class InventoryItem : ScriptableObject
{

    #region Fields and Configuration

    public string itemId;
    public string itemName;
    public Sprite icon;

    [Header("Inventory")] 
    public InventoryCategory category;

    [TextArea] 
    public string description;

    #endregion
}
