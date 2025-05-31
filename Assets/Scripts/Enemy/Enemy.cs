using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        NormalState,//正常状态
        AttackingState,//战斗状态
        MovingState,//移动状态
        RestingState//休息状态
    }
  
    public GameTaskSO gameTaskSO;
    private EnemyState currentState = EnemyState.NormalState;
    private Animator anim;
    public GameObject player; // 玩家的 Transform 组件
    private float detectionRange =70; // 检测玩家的范围
    public float idleTime = 5f;
    public float restTime = 3f;
    public float attackTime = 2f;
    private float idleTimer = 0f;
    private float restTimer = 0f;
    private float attackTimer = 0;
    public int attackrange = 5;
    private bool isMovingToRandomPosition = false;

    public  NavMeshAgent enemyAgent;
    
    
    public int HP = 100;
    public int exp = 20;
    public int Defense;
    public int attack;
    // 缓存常用组件
    private Collider enemyCollider;

    // Start is called before the first frame update
  public   void Start()
    {
        enemyCollider = GetComponent<Collider>();
      
        anim = GetComponent<Animator>();

    }



    // Update is called once per frame

   public void Update()
    {
        if (enemyAgent == null || anim == null || player == null)
        {
            return;
        }

        // 检查是否发现玩家
       if (Vector3.Distance(transform.position, player.transform. position) <= detectionRange)
        {

            if (player != null && enemyAgent != null)
            {
                enemyAgent.SetDestination(player.transform.position);
                anim.SetBool("isSleep", false);
                anim.SetBool("isMoving", true);
                anim.SetBool("isAttacking", false);
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackrange)
            {
                currentState = EnemyState.AttackingState;
                anim.SetBool("isMoving", false);
                anim.SetBool("isSleep", false);
                anim.SetBool("isAttacking", true);
                enemyAgent.isStopped = true;
                attackTimer += Time.deltaTime;
                if (attackTimer > attackTime)
                {
                    int damage = attack -(int) (PlayerProperty.Instance.defenseValue * 0.2);
                    PlayerProperty.Instance.RemoveProperty(PropertyType.HPValue, damage);
                    attackTimer = 0;
                }
              
                return;
            }
            else
            {
                anim.SetBool("isAttacking", false);
                enemyAgent.isStopped = false;
            }

        }
      

        switch (currentState)
        {
            case EnemyState.NormalState:
                idleTimer += Time.deltaTime;
                if (idleTimer > idleTime)
                {
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isSleep", true);
                    anim.SetBool("isAttacking", false);
                    currentState = EnemyState.RestingState;
                    idleTimer = 0; // 重置计时
                }
                break;
            case EnemyState.RestingState:
                restTimer += Time.deltaTime;
                if (restTimer > restTime)
                {
                    anim.SetBool("isSleep", false);
                    anim.SetBool("isMoving", true);
                    anim.SetBool("isAttacking", false);
                    currentState = EnemyState.MovingState;
                    restTimer = 0; // 重置计时

                    // 生成随机位置并设置导航目标
                    Vector3 randomPosition = FindValidRandomPosition();
                   
                        enemyAgent.SetDestination(randomPosition);
                        isMovingToRandomPosition = true;
                    
                }
                break;
            case EnemyState.MovingState:
                if (isMovingToRandomPosition && enemyAgent.pathStatus == NavMeshPathStatus.PathComplete && enemyAgent.remainingDistance <5)
                {
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isSleep", false);
                    anim.SetBool("isAttacking", false);
                    currentState = EnemyState.NormalState;
                    isMovingToRandomPosition = false;
                }
                break;
            case EnemyState.AttackingState:
                // 可以在这里添加攻击逻辑，例如播放攻击动画、造成伤害等
               // playerProperty.RemoveProperty(PropertyType.HPValue, 20);
                break;
            default:
                break;
        }
    }

   public  Vector3 FindValidRandomPosition()
    {
       
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            Vector3 randomPosition = transform.position + randomDir * Random.Range(20f, 30f);
       
            return randomPosition;
    }
public void TakeDamage(int damage)
    {
        int Defensevalue = (int)(Defense * 0.2);
        HP -= (damage-Defensevalue);
        if (HP <= 0)
        {
            Die();
          
        }
    }

    private void Die()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        } 

        const int itemCount = 4;
        for (int i = 0; i < itemCount; i++)
        {
            SpawnPickableItem();
        }
        if (gameTaskSO.state == GameTaskState.Executing||HP<=0) {
            Debug.Log($"{gameObject.name} 死亡，准备触发事件");
            EventCenter.EnemyDied(this);
        }

        Destroy(gameObject);
    }

    private void SpawnPickableItem()
    {
        ItemSO item = ItemDBManager.Instance?.GetRandomItem();
        if (item == null)
        {
            Debug.LogError("未能从 ItemDBManager 获取随机物品");
            return;
        }

        GameObject go = Instantiate(item.prefab, transform.position, Quaternion.identity);
        go.tag = Tag.INTERACTABLE;

        Animator anim = go.GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        PickableObject po = go.AddComponent<PickableObject>();
        po.itemSO = item;

        Collider collider = go.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
            collider.isTrigger = false;
        }

        Rigidbody rgd = go.GetComponent<Rigidbody>();
        if (rgd != null)
        {
            rgd.isKinematic = false;
            rgd.useGravity = true;
        }
    }
}