using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates tutorial state and operations for the game.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    private readonly HashSet<string> _shownTutorials = new();

    private readonly List<TutorialData> _unlockedTutorials = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool HasSeen(string tutorialId)
    {
        return !string.IsNullOrWhiteSpace(tutorialId) && _shownTutorials.Contains(tutorialId);
    }

    public void SetSeen(string tutorialId)
    {
        if (!string.IsNullOrWhiteSpace(tutorialId))
            _shownTutorials.Add(tutorialId);
    }

    public void UnlockTutorial(TutorialData tutorial)
    {
        if (tutorial == null || _unlockedTutorials.Contains(tutorial))
            return;

        _unlockedTutorials.Add(tutorial);
    }

    public bool HasUnlockedTutorial(TutorialData tutorial)
    {
        return _unlockedTutorials.Contains(tutorial);
    }

    public List<TutorialData> GetUnlockedTutorials()
    {
        return _unlockedTutorials;
    }
}
