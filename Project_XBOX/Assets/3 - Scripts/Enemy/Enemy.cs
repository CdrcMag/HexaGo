using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

// ABSTRACT CLASS OF ENEMY : CAN'T INSTANTIATE

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

public abstract class Enemy : MonoBehaviour
{
    public const string NAME_PLAYER = "Player";
    public const float DELAY_TO_START = 2f;


    // ===================== VARIABLES =====================

    [Header("Properties")]
    [SerializeField] protected float lifePoint = 50f;
    [SerializeField] protected float speed = 10f;
    protected float initialSpeed = 10f;
    [SerializeField] protected float damageOnCollision = 10f;
    [SerializeField] protected Transform target;
    [SerializeField] protected bool isActivated = false;

    [Header("Components")]
    [SerializeField] protected Eye[] eyes;

    [Header("Prefabs")]
    [SerializeField] protected GameObject ptcHitPref;
    [SerializeField] protected GameObject ptcDiePref;

    private CameraShake cameraShake;
    private RoomManager roomManager;
    private bool hasUpdated = false;
    private SoundManager soundManager;

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
        cameraShake = Camera.main.GetComponent<CameraShake>();
        roomManager = GameObject.Find("SceneManager").GetComponent<RoomManager>();
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        StartCoroutine(ActivateEnemy());

        if (!IsTargetEmpty())
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

    private IEnumerator ActivateEnemy()
    {
        yield return new WaitForSeconds(DELAY_TO_START);

        isActivated = true;
    }

    // Return TRUE if the variable "target" is null
    public virtual bool IsTargetEmpty()
    {
        bool _isEmpty = false;

        if(target == null) { _isEmpty = true; }

        return _isEmpty;
    }

    public virtual void HitPlayer()
    {
        PlayerManager player = target.GetComponent<PlayerManager>();
        player.SetLifePoint(damageOnCollision);
        soundManager.playAudioClipWithPitch(5, 0.5f);
    }

    public virtual void TakeDamage(float _damage)
    {
        lifePoint -= _damage;

        GameObject ptcHit;
        ptcHit = Instantiate(ptcHitPref, transform.position, Quaternion.identity);
        Destroy(ptcHit, 4f);

        if(lifePoint <= 0)
        {
            Die();
        }
        else
        {
            cameraShake.Shake(0.1f, 0.8f);
            soundManager.playAudioClip(5);
        }
    }

    public virtual void Die()
    {
        if (!hasUpdated)
        {
            GameObject ptcDie;
            ptcDie = Instantiate(ptcDiePref, transform.position, Quaternion.identity);
            Destroy(ptcDie, 8f);

            cameraShake.Shake(0.3f, 1.5f);
            soundManager.playAudioClip(6);

            roomManager.UpdateState();
            hasUpdated = true;
        }

        Destroy(gameObject);
    }

    // Move toward the target
    public virtual void MoveToward()
    {
        if (!isActivated)
            return;

        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    }

    // Rotate toward the target
    public virtual void RotateToward(float _speed, Transform _objToRotate)
    {
        if (!isActivated)
            return;

        // Determine which direction to rotate towards
        Vector2 targetDirection = target.position - transform.position;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _objToRotate.rotation = Quaternion.Slerp(_objToRotate.rotation, q, Time.deltaTime * _speed);
    }

    public virtual void MoveToBeInRange(float _range)
    {
        if (!isActivated)
            return;

        float dist = Vector3.Distance(target.position, transform.position);
        float step = speed * Time.deltaTime;

        if (dist > _range)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    public virtual void Shoot(GameObject _bulletPref, Transform _posToShoot, Transform _canon, float _speed)
    {
        if (!isActivated)
            return;

        GameObject bullet;
        bullet = Instantiate(_bulletPref, _posToShoot.position, _canon.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * _speed;
        Destroy(bullet, 10f);
    }
}
