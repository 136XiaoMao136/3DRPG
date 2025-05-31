using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public float[] playerStats;
    public float[] playerPositionAndRotation;
    public List<ItemSO> inventoryContent;

    public PlayerData(float[] playerStats, float[] playerPositionAndRotation, List<ItemSO> inventoryContent)
    {
        this.playerStats = playerStats;
        this.playerPositionAndRotation = playerPositionAndRotation;
        this.inventoryContent = inventoryContent;
    }

}