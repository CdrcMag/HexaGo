using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Loaded in child
    protected GameObject projectilePrefab;

    [Header("Weapon settings")]
    [SerializeField]
    protected float bulletSpeed;

    public float bulletDamage;

    [SerializeField]
    protected float minIntervalBetweenShots;

    protected bool canShoot = true;


    public virtual void Shoot()
    {
        //print($"Bullet speed : {bulletSpeed} / Bullet damage {bulletDamage} / Intervalle : {minIntervalBetweenShots}");
    }

}