using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusilPompe : Weapon
{
    private float cpt = 0;
    private SoundManager soundManager;
    private const float LIFETIME = 0.1f;
    private const float DELAY = 0.01f;

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Fusil Pompe Projectile");
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (!canShoot)
        {
            cpt += Time.deltaTime;
            if (cpt >= this.minIntervalBetweenShots)
            {
                cpt = 0;
                canShoot = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f)
        {
            if (canShoot)
            {
                this.Shoot();
                canShoot = false;
            }

        }
    }

    public override void Shoot()
    {
        base.Shoot();

        soundManager.playAudioClip(4);

        StartCoroutine(ShootShotgun());
    }

    private IEnumerator ShootShotgun()
    {
        ShootProjectile(-30f);

        yield return new WaitForSeconds(DELAY);

        ShootProjectile(-10f);

        yield return new WaitForSeconds(DELAY);

        ShootProjectile(10f);

        yield return new WaitForSeconds(DELAY);

        ShootProjectile(30f);
    }

    private void ShootProjectile(float _addRotation)
    {
        Vector3 rot;
        Quaternion currentQuaternionRot = Quaternion.identity;
        Quaternion quat;
        GameObject projectileInstance;
        Vector2 dir;

        rot = new Vector3(transform.parent.rotation.eulerAngles.x, transform.parent.rotation.y, transform.parent.rotation.z + _addRotation);
        currentQuaternionRot.eulerAngles = rot;
        quat = currentQuaternionRot;

        projectileInstance = Instantiate(projectilePrefab, transform.position, quat);
        dir = projectileInstance.transform.up * bulletSpeed;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
        Destroy(projectileInstance, LIFETIME);
    }
}
