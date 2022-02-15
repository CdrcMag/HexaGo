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
        //Ici prog le tir du shotgun
    }


}