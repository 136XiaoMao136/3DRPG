using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI descriptionText;

    public GameObject propertyGrid;
    public GameObject propertyTemplate;

    private ItemSO itemSO;
    private ItemUI itemUI;
    

    private void Start()
    {
        propertyTemplate.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void UpdateItemDetailUI(ItemSO itemSO,ItemUI itemUI)
    {
        this.itemSO = itemSO;
        this.itemUI = itemUI;
        this.gameObject.SetActive(true);

        string type = "";
        switch (itemSO.itemType)
        {
            case ItemType.Weapon:
                type = "武器"; break;
            case ItemType.Consumable:
                type = "可消耗品"; break;
        }

        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        typeText.text = type;
        descriptionText.text = itemSO.description;

        foreach(Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        foreach(Property property in itemSO.propertyList){
            string propertyStr = "";
            string propertyName = "";
            switch (property.propertyType)
            {
                case PropertyType.HPValue:
                    propertyName = "生命值：";
                    break;
                case PropertyType.DefenseValue:
                    propertyName = "防御力：";
                    break;
                case PropertyType.MentalValue:
                    propertyName = "精神值：";
                    break;
                case PropertyType.SpeedValue:
                    propertyName = "速度：";
                    break;
                case PropertyType.AttackValue:
                    propertyName = "攻击力：";
                    break;
                default:
                    break;
            }
            propertyStr += propertyName;
            propertyStr += property.value;
            GameObject go = GameObject.Instantiate(propertyTemplate);
            go.SetActive(true);
            go.transform.SetParent(propertyGrid.transform,false);
            go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
        }

        
    }

    public void OnUseButtonClick()
    {
        int index = InventoryManager.Instance.itemList.IndexOf(itemSO);
        int number = InventoryManager.Instance.itemNumber[index];
        InventoryUI.Instance.OnItemUse(itemSO, itemUI,number);
        this.gameObject.SetActive(false);
    }

}
