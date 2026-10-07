using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates inventory page behavior and its Unity lifecycle.
/// </summary>
public class InventoryPageController : MonoBehaviour
{

    #region Fields and Configuration

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


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        RefreshInventory();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Rebuilds inventory for this component.
    /// </summary>
    public void RefreshInventory()
    {
        CreateCategories();
    }

    /// <summary>
    /// Creates categories for this component.
    /// </summary>
    private void CreateCategories()
    {
        if (_categoryContainer == null || _categoryOptionPrefab == null)
            return;

        ClearCategoryOptions();
        PopulateCategoryOptions();
        SelectCategory(0);
    }

    /// <summary>
    /// Clears category options for this component.
    /// </summary>
    private void ClearCategoryOptions()
    {
        foreach (Transform child in _categoryContainer)
            Destroy(child.gameObject);

        _categoryOptions.Clear();
    }

    /// <summary>
    /// Executes the populate category options operation for this component.
    /// </summary>
    private void PopulateCategoryOptions()
    {
        foreach (InventoryCategory category in _categories)
        {
            GameObject optionObj = Instantiate(_categoryOptionPrefab, _categoryContainer);

            InventoryCategoryOptionUI optionUI = optionObj.GetComponent<InventoryCategoryOptionUI>();
            
            UISelectableOption selectable = optionObj.GetComponent<UISelectableOption>();
            
            optionUI?.SetTitle(GetCategoryName(category));

            if (selectable != null)
                _categoryOptions.Add(selectable);
        }
    }

    /// <summary>
    /// Selects category for this component.
    /// </summary>
    private void SelectCategory(int index)
    {
        _currentCategoryIndex = index;

        UpdateCategoryVisuals();
    }

    /// <summary>
    /// Refreshes category visuals for this component.
    /// </summary>
    private void UpdateCategoryVisuals()
    {
        for (int i = 0; i < _categoryOptions.Count; i++)
        {
            bool selected = i == _currentCategoryIndex;
            
            _categoryOptions[i].SetSelected(selected);
        }
    }

    /// <summary>
    /// Returns the current category name for this component.
    /// </summary>
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

    #endregion
}
