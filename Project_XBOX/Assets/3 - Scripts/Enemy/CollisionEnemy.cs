using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    // ===================== VARIABLES =====================
    [Header("Enemy Properties")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private float coeffDamage = 1;

    [Header("For Self Enemy")]
    [SerializeField] private bool isSelf = false;
    [SerializeField] private float lifePoint = 60;
    [SerializeField] private float enemyParentDamageToken = 0;
    [SerializeField] private GameObject ptcHitPref; // For Self Enemy only
    [SerializeField] private bool canPassivelyDamageParent = true;
    [SerializeField] private GameObject otherObjectToDestroy;
    [SerializeField] private bool isCandy = false;

    private Transform weaponStorage;
    private Weapon weapon;

    private float startScaleX = 0f;
    private float startScaleY = 0f;
    private float incrScaleX = 0f;
    private float incrScaleY = 0f;
    private float magnitudeScale = 1.05f;
    private bool isGrowing = false;

    private bool canSpawnCandy = false;

    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        weaponStorage = GameObject.Find("Player").transform.GetChild(2);

        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        if (isSelf)
        {
            setSelfScale();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            float damage = calculateDamage(other);

            if(damage != 0)
            {
                if(isSelf)
                {
                    if (canPassivelyDamageParent) { enemy.TakeDamage(damage * coeffDamage, isSelf); }
                }
                else
                {
                    enemy.TakeDamage(damage * coeffDamage, isSelf);
                }      
            }

            if(isSelf)
            {
                damageSelf(damage);
            }

            if (other.name == "BFG Projectile(Clone)") { StartCoroutine(IDestroyProjectile(other.gameObject)); }
            else if (other.name == "Mitraillette Projectile(Clone)") 
            { 
                StartCoroutine(IDestroyProjectile(other.gameObject));
                if (enemy.GetLifePoint() <= 3 && !isCandy && canSpawnCandy) { spawnCandy(); }
                if (enemy.GetLifePoint() <= 3 && !isCandy) { canSpawnCandy = true; } 
            }
            else { Destroy(other.gameObject); }
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

    private void setSelfScale()
    {
        startScaleX = transform.localScale.x;
        startScaleY = transform.localScale.y;

        incrScaleX = startScaleX * magnitudeScale / 40;
        incrScaleY = startScaleY * magnitudeScale / 40;
    }

    private float calculateDamage(Collider2D _bullet)
    {
        int idProjectile = _bullet.GetComponent<Base_Projectile>().id;

        weapon = weaponStorage.GetChild(idProjectile - 1).GetComponent<Weapon>();
        float damage = weapon.bulletDamage;
        damage *= coeffDamage;

        return damage;
    }

    private void damageSelf(float _damage)
    {
        GameObject ptcHit;
        ptcHit = Instantiate(ptcHitPref, transform.position, Quaternion.identity);
        Destroy(ptcHit, 4f);

        soundManager.playAudioClip(5);

        lifePoint -= _damage;

        if (lifePoint <= 0)
        {
            soundManager.playAudioClip(6);

            enemy.TakeDamage(enemyParentDamageToken, true);

            if(otherObjectToDestroy != null) { Destroy(otherObjectToDestroy); }

            Destroy(gameObject);
        }
        else
        {
            if (!isGrowing)
                StartCoroutine(GrowBody());
        }
    }

    private IEnumerator IDestroyProjectile(GameObject _proj)
    {
        yield return null;

        Destroy(_proj);
    }

    private void spawnCandy()
    {
        PlayerManager.Instance.SpawnCandyAtPos(transform.position);
    }
}
