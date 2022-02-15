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
        //Code test
        //GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        //projectileInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);

        print($"Bullet speed : {bulletSpeed} / Bullet damage {bulletDamage} / Intervalle : {minIntervalBetweenShots}");
    }

}