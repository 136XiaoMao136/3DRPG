using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameTaskDBSO : ScriptableObject
{
    public List<GameTaskSO> taskList;
    public List<GameTaskSO> tasks { get { return taskList; } }
}
