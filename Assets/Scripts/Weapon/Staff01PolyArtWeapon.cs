using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff01PolyArtWeapon : Weapon
{
    public float icebulletSpeed; // 寒冰子弹的速度
    public GameObject icebulletPrefab; // 寒冰子弹的预制体
    private GameObject icebulletGo;
    private Animator anim;
    public GameObject player;
    public Transform firePoint; // 子弹发射点
    private static string ANIM_PARM_ISATTACK = "isAttack";
    private static int ANIM_STATE_HASH_ATTACK; // 攻击动画状态的哈希值
    private bool hasFired = false; // 标志位，用于避免重复发射子弹

    private void Start()
    {
        anim = player.GetComponent<Animator>();
        // 获取攻击动画状态的哈希值
        ANIM_STATE_HASH_ATTACK = Animator.StringToHash("Attack01");
        // 提前生成子弹并隐藏
       /*  icebulletGo = GameObject.Instantiate(icebulletPrefab, firePoint.position, firePoint.rotation);
        
          icebulletGo.transform.parent = transform;
          icebulletGo.GetComponent<Collider>().enabled = false;
          icebulletGo.SetActive(false); // 隐藏子弹*/
    }

    void Update()
    {
        if (PlayerProperty.Instance.mentalValue >= 5 && Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger(ANIM_PARM_ISATTACK);
            hasFired = false; // 重置发射标志位
        }

        // 获取当前动画状态信息
        AnimatorStateInfo currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (currentStateInfo.shortNameHash == ANIM_STATE_HASH_ATTACK && !hasFired)
        {
            SpawnBulletOnAnimation();
            hasFired = true; // 标记为已发射
        }
    }

    public void SpawnBulletOnAnimation()
    {
        if (PlayerProperty.Instance.mentalValue >= 5)
        {
            if (firePoint != null)//如果发射点不为空，则生成子弹
            {
                icebulletGo = GameObject.Instantiate(icebulletPrefab, firePoint.position, firePoint.rotation);
                icebulletGo.transform.localScale = new Vector3(1, 1, 1);
                icebulletGo.transform.parent = transform;
                icebulletGo.GetComponent<Collider>().enabled = false;
                if (icebulletGo != null)//为了避免在子弹发射后的十秒后销毁又再次生成造成的异步问题
                {
                    icebulletGo.SetActive(true);
                    icebulletGo.transform.parent = null;

                    Rigidbody bulletRigidbody = icebulletGo.GetComponent<Rigidbody>();
                    if (bulletRigidbody != null)
                    {
                        bulletRigidbody.isKinematic = false;
                        bulletRigidbody.GetComponent<Collider>().enabled = true;
                        bulletRigidbody.velocity = player.transform.forward * icebulletSpeed;
                    }

                    Destroy(icebulletGo, 10);
                    icebulletGo = null; // 清理引用
                    PlayerProperty.Instance.RemoveProperty(PropertyType.MentalValue, 5);
                }
            }
        }
    }
}