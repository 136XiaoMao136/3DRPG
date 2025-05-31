using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI numberText;
    public ItemSO itemSO;

    public void InitItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogError("����� ItemSO Ϊ null���޷���ʼ����Ʒ UI��");
            return;
        }

        this.itemSO = itemSO;
        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        typeText.text = GetItemTypeString(itemSO.itemType);

    }

    public void UpdateItemQuantity(ItemSO item, int quantity)
    {
        if (item == null || numberText == null)
        {
            Debug.LogError("������Ʒ����ʱ������� ItemSO �� numberText Ϊ null��");
            return;
        }

        numberText.text = $"����: {quantity}";
    }

    private string GetItemTypeString(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return "����";
            case ItemType.Consumable:
                return "������Ʒ";
            case ItemType.pills:
                return "ҩƷ";
            default:
                return "δ֪����";
        }
    }

    public void OnClick()
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.OnItemClick(itemSO, this);
        }
        else
        {
            Debug.LogError("InventoryUI ʵ��Ϊ null���޷�������Ʒ����¼���");
        }
    }
}