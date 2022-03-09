using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private Enemy enemy;
    [SerializeField] private float coeffDamage = 1;
    private Transform weaponStorage;
    private Weapon weapon;

    // =====================================================

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        weaponStorage = GameObject.Find("Player").transform.GetChild(2);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            int idProjectile = other.GetComponent<Base_Projectile>().id;

            weapon = weaponStorage.GetChild(idProjectile - 1).GetComponent<Weapon>();
            float damage = weapon.bulletDamage;
            damage *= coeffDamage;

            if(damage != 0)
            {
                enemy.TakeDamage(damage * coeffDamage);
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            enemy.HitPlayer();
        }
    }
}
