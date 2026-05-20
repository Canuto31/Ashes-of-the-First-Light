using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    
    private HashSet<string> _shownTutorials = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
        _shownTutorials.Clear();
    }

    public bool HasSeen(string tutorialId)
    {
        return _shownTutorials.Contains(tutorialId);
    }

    public void SetSeen(string tutorialId)
    {
        _shownTutorials.Add(tutorialId);
    }
}
