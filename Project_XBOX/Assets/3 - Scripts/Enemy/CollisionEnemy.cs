using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private Enemy enemy;

    // =====================================================

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            enemy.TakeDamage(20);
        }

        if (other.CompareTag("Player"))
        {
            enemy.HitPlayer();
        }
    }
}
