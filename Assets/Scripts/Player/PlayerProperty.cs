using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProperty : MonoBehaviour
{
    public static PlayerProperty Instance { get;private set; }
    public Dictionary<PropertyType, List<Property>> propertyDict;
    public float hpValue = 100;
    public float mentalValue = 100;
    public int defenseValue;
    public float level = 1;
    public float currentExp = 0;
    private GameObject player;
    private  NavMeshAgent agent;
    public float speed;
    // 定义属性的最小值和最大值
    private float MIN_HP = 0;
    private float MIN_MENTAL = 0;
    public  float MAX_HP =200;
    public  float MAX_MENTAL = 100;

    // Start is called before the first frame update
    void Awake()
    {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); return;
            }
            Instance = this;
        DontDestroyOnLoad(gameObject);
        propertyDict = new Dictionary<PropertyType, List<Property>>();

        propertyDict.Add(PropertyType.SpeedValue, new List<Property>());
        propertyDict.Add(PropertyType.AttackValue, new List<Property>());

        AddProperty(PropertyType.SpeedValue, (int)speed );
       
    
    }
    public  void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tag.PLAYER);
        agent = player.GetComponent<NavMeshAgent>();
        speed = agent.speed;
        // 正确订阅事件
        EventCenter.OnEnemyDied += OnEnemyDied;
    }
    private void Update()
    {
        if (hpValue <= 0)
        {
            SceneManager.LoadScene("GameOver1");
        }
    }
    public void UseDrug(ItemSO itemSO)
    {
        if (itemSO.propertyList != null)
        {
            foreach (Property p in itemSO.propertyList)
            {
                AddProperty(p.propertyType, p.value);
            }
        }
    }

    // 通用的属性增减方法
    private void ModifyProperty(PropertyType pt, int value)
    {
        switch (pt)
        {
            case PropertyType.HPValue:
                hpValue = Mathf.Clamp(hpValue + value, MIN_HP, MAX_HP);
                return;
            case PropertyType.DefenseValue:
                defenseValue = defenseValue + value;
                return;
            case PropertyType.MentalValue:
                mentalValue = Mathf.Clamp(mentalValue + value, MIN_MENTAL, MAX_MENTAL);
                return;
            case PropertyType.AttackValue:
                 PlayerAttack.Instance.attack+= value;
                return;
        }

        if (propertyDict.TryGetValue(pt, out List<Property> list))
        {
            if (value > 0)
            {
                list.Add(new Property(pt, value));
            }
            else
            {
                int absValue = Mathf.Abs(value);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].value == absValue)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    public void AddProperty(PropertyType pt, int value)
    {
        ModifyProperty(pt, value);
    }

    public void RemoveProperty(PropertyType pt, int value)
    {
        ModifyProperty(pt, -value);
    }

    public  void OnDestroy()
    {
        // 正确取消订阅事
            EventCenter.OnEnemyDied -= OnEnemyDied;
        
    }
    
    public void levelUP(int exp)
    {
        this.currentExp += exp;
        while (currentExp >= level * 30)
        {
            currentExp -= level * 30;
            level++;
            defenseValue +=(int)( level * 2);
            PlayerAttack.Instance.attack += (int)(level * 2);
            MAX_HP += level * 2;
            hpValue = MAX_HP;
            MAX_MENTAL += level * 2;
            mentalValue = MAX_MENTAL;
        }
    }
    private void OnEnemyDied(Enemy enemy)
    {
        this.currentExp += enemy.exp;

        while (currentExp >= level * 30)
        {
            currentExp -= level * 30;
            level++;
            defenseValue += (int)(level * 2);
            PlayerAttack.Instance.attack += (int)(level * 2);
            MAX_HP += level * 2;
            hpValue = MAX_HP;
            MAX_MENTAL += level * 2;
            mentalValue = MAX_MENTAL;

        }
       
        if (PlayerPropertyUI.Instance != null)
        {
            PlayerPropertyUI.Instance.UpdatePlayerPropertyUI();
        }
    }

}