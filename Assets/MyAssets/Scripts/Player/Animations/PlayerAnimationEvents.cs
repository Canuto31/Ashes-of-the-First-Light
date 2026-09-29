using System;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    public void EndAttack()
    {
        _playerController.EndAttack();
    }
}
