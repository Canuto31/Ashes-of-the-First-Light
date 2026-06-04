using System;
using System.Collections.Generic;
using UnityEngine;

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
            
            optionUI.SetTitle(GetCategoryName(category));
            
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

    private string GetCategoryName(InventoryCategory category)
    {
        switch (category)
        {
            case InventoryCategory.All:
                return "All";
            case InventoryCategory.Consumable:
                return "Consumables";

            case InventoryCategory.Equipment:
                return "Equipment";

            case InventoryCategory.KeyItem:
                return "Key Items";

            case InventoryCategory.Material:
                return "Materials";
        }

        return "";
    }
}
