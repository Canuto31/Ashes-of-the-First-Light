using UnityEngine;

[CreateAssetMenu(menuName = "Game/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemId;
    public string itemName;
    public Sprite icon;
    public string description;
}
