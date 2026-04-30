using UnityEngine;

[CreateAssetMenu(fileName = "NoteData", menuName = "Game/NoteData")]
public class NoteData : ScriptableObject
{
    [Header("Note Info")]
    public string noteTitle;

    [TextArea(5, 10)]
    public string[] pages;
}
