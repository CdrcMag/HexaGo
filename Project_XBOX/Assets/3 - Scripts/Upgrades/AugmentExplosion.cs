using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentExplosion : MonoBehaviour
{
    float augmentSpeed = 1.1f;
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
        if(collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent.GetComponent<Enemy>().TakeDamage(Totems.Instance.DamageOnExplosion);
        }
    }
}
