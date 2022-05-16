using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CroissantDeLune : Weapon
{

    private SoundManager soundManager;
    private float cpt = 0;

    [Header("Croissant de lune stats")]
    private GameObject rotator;
    public int nbrOfProjectile = 3;


    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Projectile Croissant");
        this.rotator = Resources.Load<GameObject>("Projectiles/Rotator");
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (!canShoot)
        {
            cpt += Time.deltaTime;
            if (cpt >= this.minIntervalBetweenShots)
            {
                cpt = 0;
                canShoot = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f && Pause_System.Instance.GetPauseState() == false)
        {
            if (canShoot)
            {
                this.Shoot();
                canShoot = false;
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        soundManager.playAudioClip(21);

        //Vector2[] targets = new Vector2[3];

        //targets[0] = (Vector2)transform.position + (Vector2)transform.up * 2f;
        //targets[1] = (Vector2)transform.position + (Vector2)transform.up * 3f;
        //targets[2] = (Vector2)transform.position + (Vector2)transform.up * 4f;
        List<Vector2> targets = new List<Vector2>();

        Vector2 playerPosition = transform.parent.parent.parent.position;

        for (int i = 0; i < nbrOfProjectile; i++)
        {
            targets.Add((Vector2)transform.position + (Vector2)transform.up *  (i+1));

            Vector2 midPoint = Vector2.Lerp(playerPosition, targets[i], 0.5f);
            GameObject rotatorInstance = Instantiate(rotator, midPoint, transform.rotation);
            GameObject a = Instantiate(projectilePrefab, playerPosition, transform.parent.rotation, rotatorInstance.transform);
            a.GetComponent<Projectile_Croissant>().damageOnHit = bulletDamage;

            StartCoroutine(IRotate(rotatorInstance.transform, bulletSpeed, 0.2f));
        }
    }

    
    IEnumerator IRotate(Transform t, float speed, float BulletTimeAlive)
    {
        float cpt = 0;
        while(cpt < BulletTimeAlive)
        {
            if (t != null) { t.Rotate(Vector3.forward, speed); }


            cpt += Time.deltaTime;
            yield return null;
        }    
    }
}
