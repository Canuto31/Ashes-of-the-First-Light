using System.Collections.Generic;
using UnityEngine;

public class TutorialCodexManager : MonoBehaviour
{
    public static TutorialCodexManager Instance { get; private set; }

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

    public void UnlockTutorial(TutorialData tutorial)
    {
        if (tutorial == null || HasTutorial(tutorial))
            return;

        _unlockedTutorials.Add(tutorial);
    }

    public List<TutorialData> GetUnlockedTutorials()
    {
        return _unlockedTutorials;
    }
    
    public bool HasTutorial(TutorialData tutorial)
    {
        return tutorial != null && _unlockedTutorials.Contains(tutorial);
    }
}
