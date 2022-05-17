using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbeSaoul : Enemy
{
    private const float DELAY = 0.01f;
    private const float FORCE = 100f;
    private const float TORQUE = 20f;
    private const float SPEED_JEWEL = 4f;
    private const float SPEED_COIN = 4f;

    private const float HEALTH_TO_PROC_PHASE_01 = 600f;

    private const float XMAX = 4.5f;
    private const float XMIN = -4.5f;

    // ===================== VARIABLES =====================


    [Header("Prefab")]
    [SerializeField] private GameObject SwordPref;

    public Transform SpawnPos1;
    public Transform SpawnPos2;



    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
        base.SetMaxLifePoint();
    

    }

    private void Start()
    {

        StartCoroutine(MoveBarbeSaoul());
        StartCoroutine(Attack());
    }


    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
        if (lifePoint <= 0)
        {
            base.Die();
            base.soundManager.playAudioClipWithPitch(28, 5f);
        }
    }



    private IEnumerator MoveBarbeSaoul()
    {
        float yStartPos = transform.position.y;
        float yMaxPos = yStartPos + 0.5f;
        float addPosX = 0.03f;

        float xCurrentPos = transform.position.x;



        if(xCurrentPos < XMAX && base.target.position.x > xCurrentPos)
        {
            while (xCurrentPos < XMAX && base.target.position.x > xCurrentPos)
            {
                xCurrentPos = transform.position.x;
                
                yield return new WaitForSeconds(DELAY);
                transform.position = new Vector2(transform.position.x + addPosX, transform.position.y);
                xCurrentPos = transform.position.x;
                
            }
        } 

        else if(base.target.position.x < xCurrentPos && xCurrentPos> XMIN)
        {
            while (base.target.position.x < xCurrentPos && xCurrentPos > XMIN)
            {
                yield return new WaitForSeconds(DELAY);

                transform.position = new Vector2(transform.position.x - addPosX, transform.position.y);
                xCurrentPos = transform.position.x;

            }
        }


        yield return new WaitForSeconds(0.1f);

        StartCoroutine(MoveBarbeSaoul());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        SwordSpawn();
        yield return new WaitForSeconds(1f);
        SwordSpawn();
    }

    
    private void spawnParticleAtBottom(GameObject _ptcPref, bool _mustDestroy)
    {
        GameObject ptc;
        ptc = Instantiate(_ptcPref, new Vector2(0f, -6.2f), Quaternion.identity);

        if (_mustDestroy) { Destroy(ptc, 21f); }
    }



    private void chooseAttack()
    {
        soundManager.playAudioClip(18);

        int choice = Random.Range(0, 2);

        if (choice == 0) { SwordSpawn(); }

    }
    
    public void SwordSpawn()
    {
        
        GameObject Sword1;
        GameObject Sword2;

        Sword1 = Instantiate(SwordPref, SpawnPos1.position , Quaternion.identity, SpawnPos1);
        Sword2 = Instantiate(SwordPref, SpawnPos2.position, Quaternion.identity, SpawnPos2);
        base.soundManager.playAudioClipWithPitch(29, 2f);

    }

}
