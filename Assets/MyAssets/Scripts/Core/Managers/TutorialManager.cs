using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private HashSet<string> _shownTutorials = new();

    private List<TutorialData> _unlockedTutorials = new();

    private void Awake()
    {
        Instance = this;

        _shownTutorials.Clear();
        _unlockedTutorials.Clear();
    }

    public bool HasSeen(string tutorialId)
    {
        return _shownTutorials.Contains(tutorialId);
    }

    public void SetSeen(string tutorialId)
    {
        _shownTutorials.Add(tutorialId);
    }

    public void UnlockTutorial(TutorialData tutorial)
    {
        if (_unlockedTutorials.Contains(tutorial))
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