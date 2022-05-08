using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Drone : Base_Projectile
{

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }


    [HideInInspector] public float damageOnHit = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy e = null;

            //Applique les dégâts sur l'ennemi
            if (collision.gameObject.name.Contains("spin"))
            {
                e = collision.transform.parent.parent.GetComponent<Enemy>();
            }
            else
            {
                e = collision.transform.parent.GetComponent<Enemy>();
            }

            if(e != null)
            {
                e.TakeDamage(damageOnHit);
            }

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(transform.GetChild(0).gameObject);

        }

        


        
    }
}
