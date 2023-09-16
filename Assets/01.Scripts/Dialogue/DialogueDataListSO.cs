using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDataList", menuName = "SO/Dialogue/DataList")]
public class DialogueDataListSO : ScriptableObject
{
    public List<DialogueDataSO> DialogueDatas;
}
