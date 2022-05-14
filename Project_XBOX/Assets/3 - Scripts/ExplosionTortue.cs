using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTortue : MonoBehaviour
{
    float augmentSpeed = 1.12f;
    private float timeToDestroy = 0.5f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void Update()
    {
        transform.localScale = transform.localScale * augmentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
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

            if (e != null)
            {
                //apply
                e.TakeDamage(150);
            }



        }
    }
}
