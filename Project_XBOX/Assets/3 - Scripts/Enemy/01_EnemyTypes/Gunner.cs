using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    private const float ROTATE_SPEED_CANON = 10f;

    // ===================== VARIABLES =====================

    private float rotatingSpeed = -200f;
    private float range = 5f;
    private float delay = 4f;
    private float bulletSpeed = 3f;
    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform spin;
    [SerializeField] private Transform canon;
    [SerializeField] private Transform posToShoot;
    [SerializeField] private GameObject bulletPref;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    private void Start()
    {
        StartCoroutine(ShootByDelay());
    }

    private void Update()
    {
        base.MoveToBeInRange(range);
        base.RotateToward(ROTATE_SPEED_CANON, canon);

        RotateSpin(rotatingSpeed);
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

        Shoot(bulletPref, posToShoot, canon, bulletSpeed);

        StartCoroutine(ShootByDelay());
    }

    public override void Shoot(GameObject _bulletPref, Transform _posToShoot, Transform _canon, float _speed)
    {
        base.Shoot(_bulletPref, _posToShoot, _canon, _speed);

        StartCoroutine(GrowCanon());
    }

    private IEnumerator GrowCanon()
    {
        while(canon.localScale.x < 1.2f)
        {
            yield return new WaitForSeconds(0.01f);
            canon.localScale = new Vector2(canon.localScale.x + 0.02f, canon.localScale.y + 0.02f);
        }

        while (canon.localScale.x > 1f)
        {
            yield return new WaitForSeconds(0.01f);
            canon.localScale = new Vector2(canon.localScale.x - 0.02f, canon.localScale.y - 0.02f);
        }
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        if (!isGrowingBody)
            StartCoroutine(GrowBody());
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1.3f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + 0.04f, body.localScale.y + 0.04f);
        }

        while (body.localScale.x > 1f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - 0.04f, body.localScale.y - 0.04f);
        }

        isGrowingBody = false;
    }
}
