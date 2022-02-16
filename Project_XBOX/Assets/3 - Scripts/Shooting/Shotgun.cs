using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Shotgun Projectile Prefab");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Shoot();
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