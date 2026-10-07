using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates tutorial codex state and operations for the game.
/// </summary>
public class TutorialCodexManager : MonoBehaviour
{

    #region Fields and Configuration

    public static TutorialCodexManager Instance { get; private set; }

    private readonly List<TutorialData> _unlockedTutorials = new();


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Executes the unlock tutorial operation for this component.
    /// </summary>
    public void UnlockTutorial(TutorialData tutorial)
    {
        if (tutorial == null || HasTutorial(tutorial))
            return;

        _unlockedTutorials.Add(tutorial);
    }

    /// <summary>
    /// Returns the current unlocked tutorials for this component.
    /// </summary>
    public List<TutorialData> GetUnlockedTutorials()
    {
        return _unlockedTutorials;
    }
    
    /// <summary>
    /// Returns whether tutorial is currently true.
    /// </summary>
    public bool HasTutorial(TutorialData tutorial)
    {
        return tutorial != null && _unlockedTutorials.Contains(tutorial);
    }

    #endregion
}
