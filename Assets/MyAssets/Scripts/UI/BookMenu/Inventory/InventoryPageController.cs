using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates inventory page behavior and its Unity lifecycle.
/// </summary>
public class InventoryPageController : MonoBehaviour
{
    [Header("Categories")] 
    [SerializeField] private Transform _categoryContainer;
    [SerializeField] private GameObject _categoryOptionPrefab;

    private readonly List<UISelectableOption> _categoryOptions = new();

    private readonly InventoryCategory[] _categories =
    {
        InventoryCategory.All,
        InventoryCategory.Consumable,
        InventoryCategory.Equipment,
        InventoryCategory.KeyItem,
        InventoryCategory.Material
    };

    private int _currentCategoryIndex;

    private void Start()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        CreateCategories();
    }

    private void CreateCategories()
    {
        if (_categoryContainer == null || _categoryOptionPrefab == null)
            return;

        foreach (Transform child in _categoryContainer)
        {
            Destroy(child.gameObject);
        }
        
        _categoryOptions.Clear();

        foreach (InventoryCategory category in _categories)
        {
            GameObject optionObj = Instantiate(_categoryOptionPrefab, _categoryContainer);

            InventoryCategoryOptionUI optionUI = optionObj.GetComponent<InventoryCategoryOptionUI>();
            
            UISelectableOption selectable = optionObj.GetComponent<UISelectableOption>();
            
            optionUI?.SetTitle(GetCategoryName(category));

            if (selectable != null)
                _categoryOptions.Add(selectable);
        }

        SelectCategory(0);
    }

    private void SelectCategory(int index)
    {
        _currentCategoryIndex = index;

        UpdateCategoryVisuals();
    }

    private void UpdateCategoryVisuals()
    {
        for (int i = 0; i < _categoryOptions.Count; i++)
        {
            bool selected = i == _currentCategoryIndex;
            
            _categoryOptions[i].SetSelected(selected);
        }
    }

    private static string GetCategoryName(InventoryCategory category)
    {
        return category switch
        {
            InventoryCategory.All => "All",
            InventoryCategory.Consumable => "Consumables",
            InventoryCategory.Equipment => "Equipment",
            InventoryCategory.KeyItem => "Key Items",
            InventoryCategory.Material => "Materials",
            _ => string.Empty
        };
    }
}
