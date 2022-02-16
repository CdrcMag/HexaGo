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

    [SerializeField]
    protected float bulletDamage;

    [SerializeField]
    protected float minIntervalBetweenShots;



    public virtual void Shoot()
    {
        //print($"Bullet speed : {bulletSpeed} / Bullet damage {bulletDamage} / Intervalle : {minIntervalBetweenShots}");
    }

}