using UnityEngine;

public class CroissantDeLune : Weapon
{

    private SoundManager soundManager;
    private float cpt = 0;

    [Header("Croissant de lune stats")]
    public string temp = "";
    private GameObject rotator;

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Projectile Croissant");
        this.rotator = Resources.Load<GameObject>("Projectiles/Rotator");
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

        soundManager.playAudioClip(3);

        Vector2[] targets = new Vector2[3];

        targets[0] = (Vector2)transform.position + (Vector2)transform.up * 2f;
        targets[1] = (Vector2)transform.position + (Vector2)transform.up * 3f;
        targets[2] = (Vector2)transform.position + (Vector2)transform.up * 4f;

        //GameObject a = Instantiate(projectilePrefab, targets[0], transform.parent.rotation);
        //Instantiate(projectilePrefab, targets[1], transform.parent.rotation);
        //Instantiate(projectilePrefab, targets[2], transform.parent.rotation);

        Vector2 playerPosition = transform.parent.parent.parent.position;
        Quaternion playerRotation = transform.parent.parent.parent.rotation;
        Vector2 midPoint = Vector2.Lerp(playerPosition, targets[0], 0.5f);

        Quaternion newRotation = Quaternion.Euler(playerRotation.x, playerRotation.y, playerRotation.z - 180);

        GameObject rotatorInstance = Instantiate(projectilePrefab, midPoint, newRotation);

        GameObject a = Instantiate(projectilePrefab, playerPosition, transform.parent.rotation);
        GameObject b = Instantiate(projectilePrefab, midPoint, transform.parent.rotation);
        GameObject c = Instantiate(projectilePrefab, targets[0], transform.parent.rotation);

    }


}
