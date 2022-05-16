using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Weapon
{


    private float cpt = 0;
    private SoundManager soundManager;

    [Header("Drone settings")]
    public float distance = 2;
    public float FirstPhaseSpeed = 4f;

    private List<Transform> bullets = new List<Transform>();

    private void Awake()
    {
        this.projectilePrefab = Resources.Load<GameObject>("Projectiles/Drone Projectile");
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

        soundManager.playAudioClip(20);

        
        Vector2[] targetPoints = new Vector2[5];

        float playerX = transform.root.position.x;
        float playerY = transform.root.position.y;

        targetPoints[0] = new Vector2(playerX, playerY);//Player's position
        targetPoints[1] = new Vector2(playerX, playerY) + (Vector2)transform.root.up;
        targetPoints[2] = new Vector2(playerX, playerY) + -(Vector2)transform.root.up;
        targetPoints[3] = new Vector2(playerX, playerY) + (Vector2)transform.root.right;
        targetPoints[4] = new Vector2(playerX, playerY) + -(Vector2)transform.root.right;

        FirstPhase(targetPoints);



    }

    private void FirstPhase(Vector2[] points)
    {

        for(int i = 0; i < 4; i++)
        {
            GameObject bullet = Instantiate(projectilePrefab, points[0], Quaternion.identity);
            bullet.GetComponent<Projectile_Drone>().damageOnHit = this.bulletDamage;
            StartCoroutine(IMoveToTarget(bullet.transform, points[i+1], FirstPhaseSpeed));
            bullets.Add(bullet.transform);
        }

    }

    IEnumerator IMoveToTarget(Transform what, Vector2 where, float speed)
    {
        Transform EnemyPool = GameObject.Find("EnemyPool").transform;
        GameObject boss = GameObject.Find("[Poulpy](Clone)");

        if(boss == null && EnemyPool.childCount == 0)
        {
            Destroy(what.gameObject);
            yield break;
            //Destroy(what.gameObject);
        }

        //The projectile goes to the initial position
        while (Vector2.Distance(what.position, where) >= 0.001f)
        {
            what.position = Vector2.Lerp(what.position, where, speed * Time.deltaTime);

            yield return null;
        }

        what.position = where;

        

        
        if (boss) 
        {
            //print("There is a boss");
            Vector2 target = new Vector2(0, -2.5f);
            while(Vector2.Distance(what.position, target) > 0.1f)
            {
                what.position = Vector2.LerpUnclamped(what.position, target, 10 * Time.deltaTime);
                yield return null;
            }
        }
        else if (EnemyPool.childCount > 0)
        {

            List<Transform> enemyList = new List<Transform>();
            foreach(Transform t in EnemyPool)
            {
                if(t.name.Contains("Mine") == false)
                {
                    enemyList.Add(t);
                }
            }

            Transform enemyTarget = enemyList[Random.Range(0, enemyList.Count)];
            //R�cup�rer l'information de la room (s'il y a un boss)

            //Then, the projectile targets an enemy and moves towards it
            while (what != null && enemyTarget != null)
            {
                if (enemyTarget != null && what != null)
                {
                    if (Vector2.Distance(what.position, enemyTarget.position) > 0.1f)
                    {
                        what.position = Vector2.LerpUnclamped(what.position, enemyTarget.position, 10 * Time.deltaTime);
                        if (enemyTarget == null) Destroy(what.gameObject);
                        
                    }
                    else
                    {
                        Destroy(what.gameObject);
                    }
                   
                    yield return null;
                }
                
            }

            if (what) Destroy(what.gameObject);
            
            


        }



       
        
    }


}
