using System.Collections.Generic;
using UnityEngine;

public enum ContentsType
{
    Next,
    Clear
}

[System.Serializable]
public struct Data
{
    public string Content;
    public ContentsType type;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "SO/Dialogue/Data")]
public class DialogueDataSO : ScriptableObject
{
    public List<Data> data;
}
