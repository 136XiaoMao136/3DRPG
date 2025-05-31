using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[System.Serializable]
public class AllGameData
{
    public int version;
    public PlayerData playerData;
    public string currentScene;
    public List<GameTaskSO> taskStatus;
}