using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_BFG : Base_Projectile
{
    [SerializeField] private GameObject ptcExplodePref;
    [SerializeField] private GameObject damageZonePref;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Block"))
        {
            GameObject ptcExplode;
            ptcExplode = Instantiate(ptcExplodePref, transform.position, Quaternion.identity);
            Destroy(ptcExplode, 4f);

            //GameObject damageZone;
            //damageZone = Instantiate(damageZonePref, transform.position, Quaternion.identity);
        }

        base.OnCollisionEnter2D(collision);
    }
}
