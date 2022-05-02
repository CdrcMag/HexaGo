using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratCanon : MonoBehaviour
{
    private const float ROTATE_SPEED_CANON = 10f;
    public const string NAME_PLAYER = "Player";

    // ===================== VARIABLES =====================

    private float rotatingSpeed = -200f;
    private float delay = 4f;
    private float bulletSpeed = 5f;

    [Header("Components")]
    [SerializeField] private Transform spin;
    [SerializeField] private Transform body;
    [SerializeField] private Transform posToShoot;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private GameObject ptcExplosionPref;

    private Transform target;

    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        SetTargetInStart();
    }

    private void Start()
    {
        StartCoroutine(ShootByDelay());
    }

    private void Update()
    {
        RotateToward(ROTATE_SPEED_CANON, body);

        RotateSpin(rotatingSpeed);
    }

    public virtual void SetTargetInStart()
    {
        if (GameObject.Find(NAME_PLAYER) != null)
        {
            target = GameObject.Find(NAME_PLAYER).GetComponent<Transform>();
        }
        else
        {
            Debug.Log("There is no object called " + NAME_PLAYER + " in the scene !");
        }
    }

    public void RotateToward(float _speed, Transform _objToRotate)
    {
        // Determine which direction to rotate towards
        Vector2 targetDirection = target.position - transform.position;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 180f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _objToRotate.rotation = Quaternion.Slerp(_objToRotate.rotation, q, Time.deltaTime * _speed);
    }

    // Rotate the spin
    private void RotateSpin(float _speed)
    {
        spin.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private IEnumerator ShootByDelay()
    {
        delay = Random.Range(3f, 4.5f);
        yield return new WaitForSeconds(delay);

        Shoot(bulletPref, posToShoot, body, bulletSpeed);

        StartCoroutine(ShootByDelay());
    }

    public void Shoot(GameObject _bulletPref, Transform _posToShoot, Transform _canon, float _speed)
    {
        soundManager.playAudioClipWithPitch(1, 1.4f);

        GameObject bullet;
        bullet = Instantiate(_bulletPref, _posToShoot.position, _canon.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * _speed;
        Destroy(bullet, 10f);

        GameObject ptcExplosion;
        ptcExplosion = Instantiate(ptcExplosionPref, _posToShoot.position, _canon.rotation);
        Destroy(ptcExplosion, 10f);

        StartCoroutine(GrowCanon());
    }

    private IEnumerator GrowCanon()
    {
        while(body.localScale.x < 0.8f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + 0.02f, body.localScale.y + 0.02f);
        }

        while (body.localScale.x > 0.6f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - 0.02f, body.localScale.y - 0.02f);
        }
    }
}
