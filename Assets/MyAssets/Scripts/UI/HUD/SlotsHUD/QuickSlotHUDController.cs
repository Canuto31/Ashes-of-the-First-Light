using UnityEngine;

/// <summary>
/// Coordinates quick slot hudcontroller behavior and its Unity lifecycle.
/// </summary>
public class QuickSlotHUDController : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private QuickSlotUI[] _slots;

    [SerializeField] private int _unlockedSlots = 1;

    private PlayerInputHandler _input;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        RefreshSlots();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleDebugInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes debug input for this component.
    /// </summary>
    private void HandleDebugInput()
    {
        if (_input == null || _slots == null || _slots.Length == 0)
            return;

        if (_input.UnlockSlotPressed)
        {
            _unlockedSlots = Mathf.Clamp(_unlockedSlots + 1, 1, _slots.Length);

            RefreshSlots();
        }
        
        if (_input.LockSlotPressed)
        {
            _unlockedSlots = Mathf.Clamp(_unlockedSlots - 1, 1, _slots.Length);

            RefreshSlots();
        }
    }

    /// <summary>
    /// Rebuilds slots for this component.
    /// </summary>
    private void RefreshSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
                continue;

            QuickSlotState state = i < _unlockedSlots
                ? QuickSlotState.Unlocked
                : QuickSlotState.Locked;

            _slots[i].SetState(state);
        }
    }

    #endregion
}
