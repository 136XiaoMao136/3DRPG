using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private PlayerAttack playerAttack;


    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        
    }

    public void UseItem(ItemSO itemSO)
    {
        switch (itemSO.itemType)
        {
            case ItemType.Weapon:
                break;
            case ItemType.Consumable:
               PlayerProperty.Instance.UseDrug(itemSO);
                break;
            case ItemType.pills:
                PlayerProperty.Instance.UseDrug(itemSO);
                break;
            default:
                break;
        }
    }

}
