using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates tutorial state and operations for the game.
/// </summary>
public class TutorialManager : MonoBehaviour
{

    #region Fields and Configuration

    public static TutorialManager Instance { get; private set; }

    private readonly HashSet<string> _shownTutorials = new();

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
    /// Returns whether seen is currently true.
    /// </summary>
    public bool HasSeen(string tutorialId)
    {
        return !string.IsNullOrWhiteSpace(tutorialId) && _shownTutorials.Contains(tutorialId);
    }

    /// <summary>
    /// Updates seen for this component.
    /// </summary>
    public void SetSeen(string tutorialId)
    {
        if (!string.IsNullOrWhiteSpace(tutorialId))
            _shownTutorials.Add(tutorialId);
    }

    /// <summary>
    /// Executes the unlock tutorial operation for this component.
    /// </summary>
    public void UnlockTutorial(TutorialData tutorial)
    {
        if (tutorial == null || _unlockedTutorials.Contains(tutorial))
            return;

        _unlockedTutorials.Add(tutorial);
    }

    /// <summary>
    /// Returns whether unlocked tutorial is currently true.
    /// </summary>
    public bool HasUnlockedTutorial(TutorialData tutorial)
    {
        return _unlockedTutorials.Contains(tutorial);
    }

    /// <summary>
    /// Returns the current unlocked tutorials for this component.
    /// </summary>
    public List<TutorialData> GetUnlockedTutorials()
    {
        return _unlockedTutorials;
    }

    #endregion
}
