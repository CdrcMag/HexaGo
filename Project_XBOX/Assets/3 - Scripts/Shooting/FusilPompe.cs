using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusilPompe : Weapon
{
    private float cpt = 0;
    private SoundManager soundManager;

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Fusil Pompe Projectile");
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f)
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

        soundManager.playAudioClip(4);

        //Spawns a bullet
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.parent.rotation);

        //Sets direction vector and applies it
        Vector2 dir = projectileInstance.transform.up * bulletSpeed;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

        //Destroys instance if not destroyed before
        Destroy(projectileInstance, 10f);
    }
}
