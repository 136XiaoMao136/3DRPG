
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPropertyUI : MonoBehaviour
{
    public static PlayerPropertyUI Instance { get; private set; }

    private GameObject uiGameObject;

    private Image hpProgressBar;
    private TextMeshProUGUI hpText;

    private Image levelProgressBar;
    private TextMeshProUGUI levelText;

    private GameObject propertyGrid;
    private GameObject propertyTemplate;
    private Image weaponIcon;


    
    private void Awake()
    {
        if(Instance!=null &&Instance != this)
        {
            Destroy(gameObject);return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;
        hpProgressBar = transform.Find("UI/HPProgressBar/ProgressBar").GetComponent<Image>();
        hpText = transform.Find("UI/HPProgressBar/HPText").GetComponent<TextMeshProUGUI>();
        levelProgressBar = transform.Find("UI/LevelProgressBar/ProgressBar").GetComponent<Image>();
        levelText = transform.Find("UI/LevelProgressBar/LevelText").GetComponent<TextMeshProUGUI>();

        propertyGrid = transform.Find("UI/PropertyGrid").gameObject;
        propertyTemplate = transform.Find("UI/PropertyGrid/PropertyTemplate").gameObject;
        weaponIcon = transform.Find("UI/WeaponIcon").GetComponent<Image>();

        propertyTemplate.SetActive(false);

        GameObject player= GameObject.FindGameObjectWithTag(Tag.PLAYER);
        UpdatePlayerPropertyUI();
        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdatePlayerPropertyUI();
            if (uiGameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }

        }
    }

    public void UpdatePlayerPropertyUI()
    {
        hpProgressBar.fillAmount = PlayerProperty.Instance.hpValue / PlayerProperty.Instance.MAX_HP;
        hpText.text = PlayerProperty.Instance.hpValue+ "/"+PlayerProperty.Instance.MAX_HP;

        levelProgressBar.fillAmount = PlayerProperty.Instance.currentExp*1.0f / (PlayerProperty.Instance.level*30);
        levelText.text = PlayerProperty.Instance.level.ToString();

        ClearGrid();

        AddProperty("防御力：" + PlayerProperty.Instance.defenseValue);
        AddProperty("精神值：" + PlayerProperty.Instance.mentalValue);
        AddProperty("速度：" + PlayerProperty.Instance.speed);
        AddProperty("攻击力：" + PlayerAttack.Instance.attack);


    }
    private void ClearGrid()
    {
        foreach (Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void AddProperty( string propertyStr)
    {
        GameObject go = GameObject.Instantiate(propertyTemplate);
        go.SetActive(true);
        go.transform.SetParent(propertyGrid.transform, false);

        go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
    }

    private void Show()
    {
        uiGameObject.SetActive(true);
    }
    private void Hide()
    {
        uiGameObject.SetActive(false);
    }
}
