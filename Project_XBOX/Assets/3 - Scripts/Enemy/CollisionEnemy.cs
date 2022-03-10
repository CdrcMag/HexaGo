using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private Enemy enemy;
    [SerializeField] private float coeffDamage = 1;
    [SerializeField] private bool isSelf = false;
    [SerializeField] private GameObject ptcHitPref;
    [SerializeField] private float lifePoint = 60;

    private Transform weaponStorage;
    private Weapon weapon;

    private float startScaleX = 0f;
    private float startScaleY = 0f;
    private float incrScaleX = 0f;
    private float incrScaleY = 0f;
    private float magnitudeScale = 1.05f;
    private bool isGrowing = false;

    // =====================================================

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        weaponStorage = GameObject.Find("Player").transform.GetChild(2);

        if(isSelf)
        {
            startScaleX = transform.localScale.x;
            startScaleY = transform.localScale.y;

            incrScaleX = startScaleX * magnitudeScale / 40;
            incrScaleY = startScaleY * magnitudeScale / 40;
        }
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
                enemy.TakeDamage(damage * coeffDamage, isSelf);
            }

            if(isSelf)
            {
                GameObject ptcHit;
                ptcHit = Instantiate(ptcHitPref, transform.position, Quaternion.identity);
                Destroy(ptcHit, 4f);

                lifePoint -= damage;

                if(lifePoint <= 0)
                {
                    enemy.TakeDamage(100f, true);
                    Destroy(gameObject);
                }
                else
                {
                    if (!isGrowing)
                        StartCoroutine(GrowBody());
                }
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            enemy.HitPlayer();
        }
    }

    private IEnumerator GrowBody()
    {
        isGrowing = true;
        int cpt = 0;

        while(cpt < 10)
        {
            if(gameObject != null)
                transform.localScale = new Vector2(transform.localScale.x + incrScaleX * 1.1f, transform.localScale.y + incrScaleY * 1.1f);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < 10)
        {
            if (gameObject != null)
                transform.localScale = new Vector2(transform.localScale.x - incrScaleX * 1.1f, transform.localScale.y - incrScaleY * 1.1f);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        isGrowing = false;
    }
}
