using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Game/TutorialData")]
public class TutorialData : ScriptableObject
{
    public string tutorialId;

    [Header("Book")] 
    public string title;

    [TextArea(3, 10)] 
    public string description;
    
    [Header("Popup")]
    [TextArea(2, 5)]
    public string popupMessage;

    public Sprite icon;
}
