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
            Debug.LogError("传入的 ItemSO 为 null，无法初始化物品 UI。");
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
            Debug.LogError("更新物品数量时，传入的 ItemSO 或 numberText 为 null。");
            return;
        }

        numberText.text = $"数量: {quantity}";
    }

    private string GetItemTypeString(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return "武器";
            case ItemType.Consumable:
                return "可消耗品";
            case ItemType.pills:
                return "药品";
            default:
                return "未知类型";
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
            Debug.LogError("InventoryUI 实例为 null，无法处理物品点击事件。");
        }
    }
}