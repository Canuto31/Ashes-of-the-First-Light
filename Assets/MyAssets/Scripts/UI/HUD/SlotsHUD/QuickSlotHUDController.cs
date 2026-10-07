using UnityEngine;

/// <summary>
/// Coordinates quick slot hudcontroller behavior and its Unity lifecycle.
/// </summary>
public class QuickSlotHUDController : MonoBehaviour
{
    [SerializeField] private QuickSlotUI[] _slots;

    [SerializeField] private int _unlockedSlots = 1;

    private PlayerInputHandler _input;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        RefreshSlots();
    }

    private void Update()
    {
        HandleDebugInput();
    }

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
}
