using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPick : MonoBehaviour
{

    private Animator anim;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tag.INTERACTABLE)
        {
        
            PickableObject po = collision.gameObject.GetComponent<PickableObject>();

            if (po != null&&Input.GetKeyDown(KeyCode.D))
            {
            
                InventoryManager.Instance.AddItem(po.itemSO);
                Destroy(po.gameObject);
            }

        }
    }

}
