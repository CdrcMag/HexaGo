using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TORTUGA : MonoBehaviour
{

    private GameObject target;

    [Header("Tortue Stats")]
    public float distanceToStop = 1f;
    public float movementSpeed = 1f;
    private GameObject explosion;

    private void Start()
    {
        explosion = Resources.Load<GameObject>("ExplosionTortue");

        AquireTarget();
    }


    private void Update()
    {
        if(HasTarget())
        {
            if(Vector2.Distance(transform.position, target.transform.position) > distanceToStop)
            {
                //transform.position = Vector2.Lerp(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);

                Vector3 diff = target.transform.position - transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }
            else
            {
                target = null;
                GoodByeTurtle();
            }
                
        }
    }

    private void GoodByeTurtle()
    {
        //Destroy
        GameObject a = Instantiate(explosion, transform.position, Quaternion.identity, transform);
        Destroy(gameObject, 0.45f);

    }


    private bool HasTarget()
    {
        if(target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AquireTarget()
    {
        Transform enemyPool = GameObject.Find("EnemyPool").transform;
        
        List<Transform> enemyList = new List<Transform>();
        foreach (Transform t in enemyPool)
        {
            if (t.name.Contains("Mine") == false)
            {
                enemyList.Add(t);
            }
        }

        int childToGet = Random.Range(0, enemyList.Count);

        if (enemyPool)
            target = enemyList[childToGet].gameObject;
    }

}
