using UnityEngine;

[CreateAssetMenu(fileName = "NoteData", menuName = "Game/NoteData")]
/// <summary>
/// Stores authored note content used at runtime.
/// </summary>
public class NoteData : ScriptableObject
{

    #region Fields and Configuration

    [Header("Note Info")]
    public string noteTitle;

    [TextArea(5, 10)]
    public string[] pages;

    #endregion
}
