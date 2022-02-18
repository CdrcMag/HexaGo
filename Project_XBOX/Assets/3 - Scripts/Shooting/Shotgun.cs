using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    private float cpt = 0;

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Shotgun Projectile Prefab");
    }

    private void Update()
    {
        if(!canShoot)
        {
            cpt += Time.deltaTime;
            if(cpt >= this.minIntervalBetweenShots)
            {
                cpt = 0;
                canShoot = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f)
        {
            if(canShoot)
            {
                this.Shoot();
                canShoot = false;
            }
            
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        //Spawns a bullet
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.parent.rotation);

        //Sets direction vector and applies it
        Vector2 dir = projectileInstance.transform.up * bulletSpeed;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

        //Destroys instance if not destroyed before
        Destroy(projectileInstance, 10f);
    }


}