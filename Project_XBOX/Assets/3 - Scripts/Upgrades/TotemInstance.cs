using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemInstance : MonoBehaviour
{
    [HideInInspector] public Totems totemMain;
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //Ajouter explosion ici
        totemMain.totalTotemPlaced -= 1;
    }
}
