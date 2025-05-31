using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        NormalState,//����״̬
        AttackingState,//ս��״̬
        MovingState,//�ƶ�״̬
        RestingState//��Ϣ״̬
    }
  
    public GameTaskSO gameTaskSO;
    private EnemyState currentState = EnemyState.NormalState;
    private Animator anim;
    public GameObject player; // ��ҵ� Transform ���
    private float detectionRange =70; // �����ҵķ�Χ
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
    // ���泣�����
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

        // ����Ƿ������
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
                    idleTimer = 0; // ���ü�ʱ
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
                    restTimer = 0; // ���ü�ʱ

                    // �������λ�ò����õ���Ŀ��
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
                // ������������ӹ����߼������粥�Ź�������������˺���
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
            Debug.Log($"{gameObject.name} ������׼�������¼�");
            EventCenter.EnemyDied(this);
        }

        Destroy(gameObject);
    }

    private void SpawnPickableItem()
    {
        ItemSO item = ItemDBManager.Instance?.GetRandomItem();
        if (item == null)
        {
            Debug.LogError("δ�ܴ� ItemDBManager ��ȡ�����Ʒ");
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