using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Drone : Base_Projectile
{
    [SerializeField] private SpriteRenderer shuriken01;
    [SerializeField] private SpriteRenderer shuriken02;
    [SerializeField] private Rotate rotShuriken01;
    [SerializeField] private Rotate rotShuriken02;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }


    [HideInInspector] public float damageOnHit = 0;

    private void Start()
    {
        StartCoroutine(IAddSpeed(rotShuriken01));
        StartCoroutine(IAddSpeed(rotShuriken02));
    }

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

            shuriken01.enabled = false;
            shuriken02.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(transform.GetChild(0).gameObject);

        }  
    }

    private IEnumerator IAddSpeed(Rotate _rot)
    {
        yield return new WaitForSeconds(0.1f);

        if(_rot.speed < 0)
        {
            _rot.speed -= 75;
        }
        else
        {
            _rot.speed += 75;
        }

        StartCoroutine(IAddSpeed(_rot));
    }
}
