using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

// ABSTRACT CLASS OF ENEMY : CAN'T INSTANTIATE

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

public abstract class Enemy : MonoBehaviour
{
    public const string NAME_PLAYER = "Player";


    // ===================== VARIABLES =====================

    [Header("Properties")]
    [SerializeField] protected float lifePoint = 50f;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float initialSpeed = 10f;
    [SerializeField] protected float damageOnCollision = 10f;
    [SerializeField] protected Transform target;

    [Header("Components")]
    [SerializeField] protected Eye[] eyes;

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _lifePoint) { lifePoint = _lifePoint; }
    public float GetLifePoint() { return lifePoint; }
    public void SetSpeed(float _speed) { speed = _speed; }
    public float GetSpeed() { return speed; }
    public void SetInitialSpeed(float _initialSpeed) { initialSpeed = _initialSpeed; }
    public float GetInitialSpeed() { return initialSpeed; }
    public void SetDamageOnCollision(float _damageOnCollision) { damageOnCollision = _damageOnCollision; }
    public float GetDamageOnCollision() { return damageOnCollision; }
    public void SetTarget(Transform _target) { target = _target; }
    public Transform GetTarget() { return target; }
    public Eye[] GetEyes() { return eyes; }
    public Eye GetEye(int _index) { return eyes[_index]; }

    // =====================================================



    // Set the variable "target" with the player in the scene
    public virtual void SetTargetInStart()
    {
        if(!IsTargetEmpty())
        {
            return;
        }

        if(GameObject.Find(NAME_PLAYER) != null)
        {
            target = GameObject.Find(NAME_PLAYER).GetComponent<Transform>();
        }
        else
        {
            Debug.Log("There is no object called " + NAME_PLAYER + " in the scene !");
        }
    }

    // Return TRUE if the variable "target" is null
    public virtual bool IsTargetEmpty()
    {
        bool _isEmpty = false;

        if(target == null) { _isEmpty = true; }

        return _isEmpty;
    }

    public virtual void HitPlayer(float _damageOnCollision)
    {
        Debug.Log("Player hit !");
        Debug.Log("Damaged : " + _damageOnCollision);
    }

    public virtual void TakeDamage(float _damage)
    {
        Debug.Log("Enemy hit !");
        Debug.Log("Damaged : " + _damage);
    }

    // Move toward the target
    public virtual void MoveToward(Transform _target, float _speed)
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, _target.position, step);
    }

    public virtual void MoveToBeInRange(Transform _target, float _speed, float _range)
    {

    }

    public virtual void ShootInRange()
    {

    }

    public virtual void ShootAround()
    {

    }
}
