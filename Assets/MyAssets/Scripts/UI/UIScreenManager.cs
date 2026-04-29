using System;
using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        var state = GameStateManager.Instance.GetState();

        if (state == GameStateManager.GameState.Menu)
        {
            if (!_pauseMenu.activeSelf)
            {
                _pauseMenu.SetActive(true);
                UI_Interaction.Instance.Hide();
                InteractionUIManager.Instance.HideVisual();
            }
        }
        else
        {
            if (_pauseMenu.activeSelf)
                _pauseMenu.SetActive(false);
        }
    }
}
