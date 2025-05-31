using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff01PolyArtWeapon : Weapon
{
    public float icebulletSpeed; // �����ӵ����ٶ�
    public GameObject icebulletPrefab; // �����ӵ���Ԥ����
    private GameObject icebulletGo;
    private Animator anim;
    public GameObject player;
    public Transform firePoint; // �ӵ������
    private static string ANIM_PARM_ISATTACK = "isAttack";
    private static int ANIM_STATE_HASH_ATTACK; // ��������״̬�Ĺ�ϣֵ
    private bool hasFired = false; // ��־λ�����ڱ����ظ������ӵ�

    private void Start()
    {
        anim = player.GetComponent<Animator>();
        // ��ȡ��������״̬�Ĺ�ϣֵ
        ANIM_STATE_HASH_ATTACK = Animator.StringToHash("Attack01");
        // ��ǰ�����ӵ�������
       /*  icebulletGo = GameObject.Instantiate(icebulletPrefab, firePoint.position, firePoint.rotation);
        
          icebulletGo.transform.parent = transform;
          icebulletGo.GetComponent<Collider>().enabled = false;
          icebulletGo.SetActive(false); // �����ӵ�*/
    }

    void Update()
    {
        if (PlayerProperty.Instance.mentalValue >= 5 && Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger(ANIM_PARM_ISATTACK);
            hasFired = false; // ���÷����־λ
        }

        // ��ȡ��ǰ����״̬��Ϣ
        AnimatorStateInfo currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (currentStateInfo.shortNameHash == ANIM_STATE_HASH_ATTACK && !hasFired)
        {
            SpawnBulletOnAnimation();
            hasFired = true; // ���Ϊ�ѷ���
        }
    }

    public void SpawnBulletOnAnimation()
    {
        if (PlayerProperty.Instance.mentalValue >= 5)
        {
            if (firePoint != null)//�������㲻Ϊ�գ��������ӵ�
            {
                icebulletGo = GameObject.Instantiate(icebulletPrefab, firePoint.position, firePoint.rotation);
                icebulletGo.transform.localScale = new Vector3(1, 1, 1);
                icebulletGo.transform.parent = transform;
                icebulletGo.GetComponent<Collider>().enabled = false;
                if (icebulletGo != null)//Ϊ�˱������ӵ�������ʮ����������ٴ�������ɵ��첽����
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
                    icebulletGo = null; // ��������
                    PlayerProperty.Instance.RemoveProperty(PropertyType.MentalValue, 5);
                }
            }
        }
    }
}