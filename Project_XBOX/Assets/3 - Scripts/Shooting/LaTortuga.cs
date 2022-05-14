using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaTortuga : Weapon
{
    private float cpt = 0;
    private SoundManager soundManager;

    private GameObject TortugaPrefab;

    private void Awake()
    {
        TortugaPrefab = Resources.Load<GameObject>("Lulu La Tortue");
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

        soundManager.playAudioClip(1);

        //Spawns a bullet
        GameObject projectileInstance = Instantiate(TortugaPrefab, transform.position, transform.parent.rotation);

        //Destroys instance if not destroyed before
        //Destroy(projectileInstance, 10f);
    }




}
