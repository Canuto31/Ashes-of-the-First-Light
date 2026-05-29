using System;
using UnityEngine;
using UnityEngine.UI;

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
        if (_input.UnlockSlotPressed)
        {
            _unlockedSlots++;
            
            _unlockedSlots = Mathf.Clamp(
                _unlockedSlots,
                1,
                _slots.Length
            );

            RefreshSlots();
        }
        
        if (_input.LockSlotPressed)
        {
            _unlockedSlots--;
            
            _unlockedSlots = Mathf.Clamp(
                _unlockedSlots,
                1,
                _slots.Length
            );

            RefreshSlots();
        }
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _unlockedSlots)
            {
                _slots[i].SetState(
                    QuickSlotState.Unlocked
                );
            }
            else
            {
                _slots[i].SetState(
                    QuickSlotState.Locked
                );
            }
        }
    }
}
