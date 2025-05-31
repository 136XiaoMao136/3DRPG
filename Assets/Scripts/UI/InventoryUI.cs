
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    private GameObject uiGameObject;
    private GameObject content;
    public GameObject itemPrefab;

    private bool isShow=false;

    public ItemDetailUI itemDetailUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
   
        uiGameObject = transform.Find("UI").gameObject;
        content = transform.Find("UI/ListBg/Scroll View/Viewport/Content").gameObject;
        Hide();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isShow)
            {
                Hide();
                isShow = false;
            }
            else
            {
                Show();
                isShow = true;
            }
        }
    }


    public void Show()
    {
        uiGameObject.SetActive(true);
    }
    public void Hide()
    {
        uiGameObject.SetActive(false);
    }
    public void AddItem(ItemSO itemSO)
    {
     
                ItemUI itemUI = itemPrefab.GetComponent<ItemUI>();

                itemUI.InitItem(itemSO);
    }
    public void OnItemClick(ItemSO itemSO,ItemUI itemUI)
    {
        itemDetailUI.UpdateItemDetailUI(itemSO,itemUI);
    }

    public void OnItemUse(ItemSO itemSO,ItemUI itemUI,int number)
    {
            if (itemUI != null&&number==1)
            {
                Destroy(itemUI.gameObject);
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RemoveItem(itemSO,number);
            }

            GameObject player = GameObject.FindGameObjectWithTag(Tag.PLAYER);
            if (player != null)
            {
                Player playerComponent = player.GetComponent<Player>();
                if (playerComponent != null)
                {
                    playerComponent.UseItem(itemSO);
                    

                }
                else
                {
                    Debug.LogError("玩家对象上未找到 Player 组件！");
                }
            }
            else
            {
                Debug.LogError("未找到玩家对象！");
            }
        }

}
