using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFuckingGun : Weapon
{

    private float cpt = 0;
    private SoundManager soundManager;
    public GameObject ptcBFGPref;

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/BFG Projectile");
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

        soundManager.playAudioClip(2);

        //Spawns a bullet
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.parent.rotation);

        //Sets direction vector and applies it
        Vector2 dir = projectileInstance.transform.up * bulletSpeed;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

        StartCoroutine(IAugmentVelocity(projectileInstance, dir));

        //Destroys instance if not destroyed before
        Destroy(projectileInstance, 10f);
    }

    private IEnumerator IAugmentVelocity(GameObject _projectile, Vector2 _dir)
    {
        yield return new WaitForSeconds(0.6f);

        if (_projectile != null)
        {
            soundManager.playAudioClipWithPitch(2, 1.5f);

            GameObject ptcBFG;
            ptcBFG = Instantiate(ptcBFGPref, _projectile.transform.position, Quaternion.identity);
            Destroy(ptcBFG, 4f);

            Vector2 dir = _dir * 4;
            _projectile.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
        }
    }
}
