using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullt : MonoBehaviour
{
    
    public int atkValue = 30;
    private Rigidbody rgd;
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Tag.PLAYER)
        {
            return;
        }

        rgd.isKinematic = true;
        col.enabled = false;

        transform.parent = collision.gameObject.transform;

        Destroy(this.gameObject, 1f);

        if (collision.gameObject.tag == Tag.ENEMY)
        {
            int damge = (int)(PlayerAttack.Instance.attack * 0.5 + atkValue * 0.5);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damge);
            Destroy(this.gameObject);
        }
    }
}

