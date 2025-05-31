using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public GameObject ItemUIPrefab;
    public Transform itemUIRoot; // ���ڷ��� ItemUI ʵ���ĸ�����

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(itemUIRoot);
        // ��ʼ���б�
        itemList = new List<ItemSO>();
        itemNumber = new List<int>();
    }

    public List<ItemSO> itemList;
    public List<int> itemNumber;

    public void AddItem(ItemSO item)
    {
        bool itemFound = false;
        int index = 0;

        // ������Ʒ�Ƿ��Ѵ���
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == item)
            {
                itemFound = true;
                index = i;
                break;
            }
        }

        if (itemFound)
        {
            // ��Ʒ�Ѵ��ڣ���������
            itemNumber[index]++;
            UpdateItemUI(item, itemNumber[index]);
        }
        else
        {
            // ��Ʒ�����ڣ��������Ʒ
            itemList.Add(item);
            itemNumber.Add(1);

            // ʵ���� ItemUI
            GameObject itemUIGO = Instantiate(ItemUIPrefab, itemUIRoot);
            ItemUI itemUI = itemUIGO.GetComponent<ItemUI>();
            itemUI.InitItem(item);
            UpdateItemUI(item, 1);
            InventoryUI.Instance.AddItem(item);
        }

        MessageUI.Instance.Show("������һ����" + item.name);
    }

    public void RemoveItem(ItemSO itemSO,int number)
    {
        int index = itemList.IndexOf(itemSO);
        if (number == 1)
        {
            itemList.RemoveAt(index);
            itemNumber.RemoveAt(index);
            ItemUI[] allItemUIs = itemUIRoot.GetComponentsInChildren<ItemUI>();
            foreach (ItemUI itemUI in allItemUIs)
            {
                if (itemUI.itemSO == itemSO)
                {
                    Destroy(itemUI.gameObject);
                    break;
                }
            }
            UpdateItemUI(itemSO, 0);
        }else if (number > 1)
        {
            itemNumber[index]--;
            UpdateItemUI(itemSO, itemNumber[index]);
        }
    }

    private void UpdateItemUI(ItemSO item, int quantity)
    {
        ItemUI[] allItemUIs = itemUIRoot.GetComponentsInChildren<ItemUI>();
        foreach (ItemUI itemUI in allItemUIs)
        {
            if (itemUI.itemSO == item)
            {
                itemUI.UpdateItemQuantity(item, quantity);
                break;
            }
        }
    }
}