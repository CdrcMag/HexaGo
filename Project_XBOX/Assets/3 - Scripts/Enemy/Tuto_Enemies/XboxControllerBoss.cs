using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxControllerBoss : Enemy
{
    private const int MAGNITUDE_MOVE = 120;
    private const float BULLET_SPEED = 4f;
    private const float BOUNCINGBULLET_SPEED = 4f;
    private const float DELAY_BOUNCINGBULLET = 4f;


    // ===================== VARIABLES =====================

    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform posToShoot;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private GameObject bouncingBulletPref;
    public Transform enemyPool;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    private void Start()
    {
        StartCoroutine(MoveLateral());
        StartCoroutine(ShootBullets());
        StartCoroutine(ShootBouncingBullets());
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        if (!isGrowingBody)
            StartCoroutine(GrowBody());
    }

    private IEnumerator ShootBullets()
    {
        float delay = Random.Range(2.5f, 4f);
        yield return new WaitForSeconds(delay);

        ShootProjectile(180f, bulletPref, BULLET_SPEED, 4f);
        ShootProjectile(205f, bulletPref, BULLET_SPEED, 4f);
        ShootProjectile(155f, bulletPref, BULLET_SPEED, 4f);
        ShootProjectile(230f, bulletPref, BULLET_SPEED, 4f);
        ShootProjectile(130f, bulletPref, BULLET_SPEED, 4f);

        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBouncingBullets()
    {
        yield return new WaitForSeconds(DELAY_BOUNCINGBULLET);

        ShootProjectile(230f, bouncingBulletPref, BOUNCINGBULLET_SPEED, -1);
        ShootProjectile(130f, bouncingBulletPref, BOUNCINGBULLET_SPEED, -1);
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1.15f)
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

    private IEnumerator MoveLateral()
    {
        int cpt = 0;

        while(cpt < MAGNITUDE_MOVE)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < MAGNITUDE_MOVE * 2)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < MAGNITUDE_MOVE)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        StartCoroutine(MoveLateral());
    }

    private void ShootProjectile(float _addRotation, GameObject _bulletPref, float _speed, float _lifetime)
    {
        Vector3 rot;
        Quaternion currentQuaternionRot = Quaternion.identity;
        Quaternion quat;
        GameObject projectileInstance;
        Vector2 dir;

        rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.y, _addRotation);
        currentQuaternionRot.eulerAngles = rot;
        quat = currentQuaternionRot;

        projectileInstance = Instantiate(_bulletPref, posToShoot.position, quat, enemyPool);
        dir = projectileInstance.transform.up * _speed;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

        if(_lifetime != -1)
        {
            Destroy(projectileInstance, _lifetime);
        }
    }
}
