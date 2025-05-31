using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : InteractableObject
{
    public ItemSO itemSO;
    public GameTaskSO gameTaskSO;

    protected override void Interact()
    {
        if (itemSO.itemType == ItemType.taskInteracter&&gameTaskSO.state==GameTaskState.Executing)
        {
            EventCenter.InteractableObject(this);
            Destroy(this.gameObject);
            MessageUI.Instance.Show("现在的拾取进度+1");
        }
        else
        {
            Destroy(this.gameObject);
            InventoryManager.Instance.AddItem(itemSO);
        }   
    }
}
