using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klovis : Enemy
{
    private const float DELAY = 0.01f;
    private const float FORCE = 100f;
    private const float TORQUE = 20f;
    private const float SPEED_JEWEL = 4f;
    private const float SPEED_COIN = 5f;

    private const float HEALTH_TO_PROC_PHASE_01 = 600f;

    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private Transform head;
    [SerializeField] private GameObject lockTreasure;
    [SerializeField] private GameObject lockTreasureSprite;

    [Header("Prefab")]
    [SerializeField] private GameObject crossTreasurePref;
    [SerializeField] private GameObject jewelPref;
    [SerializeField] private GameObject piratCoinPref;
    [SerializeField] private Sprite[] jewelSprites;

    [Header("Particles")]
    [SerializeField] private GameObject bubbleCurtainPref;
    [SerializeField] private GameObject bubbleDropPref;

    private float delayPhase = 0.5f;
    private int phase = 0;
    private int xPosChecker = 0;

    private GameObject crossTreasure;
    private GameObject bubbleCurtain;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
        base.SetMaxLifePoint();

        bubbleCurtain = Instantiate(bubbleCurtainPref, new Vector2(0f, -6.2f), Quaternion.identity);
    }

    private void Start()
    {
        spawnTreasureCross();

        StartCoroutine(MoveKlovis());

        StartCoroutine(UpdatePhase());
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    private IEnumerator AnimateHead()
    {
        float yStartPos = transform.localPosition.y;
        float yMaxPos = yStartPos + 2f;

        while (head.localPosition.y < yMaxPos)
        {
            yield return new WaitForSeconds(DELAY);

            head.localPosition = new Vector2(head.localPosition.x, head.localPosition.y + 0.05f);
        }

        while (head.localPosition.y > yStartPos)
        {
            yield return new WaitForSeconds(DELAY);

            head.localPosition = new Vector2(head.localPosition.x, head.localPosition.y - 0.2f);
        }

        chooseAttack();
    }

    private IEnumerator MoveKlovis()
    {
        float yStartPos = transform.position.y;
        float yMaxPos = yStartPos + 0.5f;
        float addPosX = 0.02f;

        if(xPosChecker == -3)
        {
            addPosX = 0.02f;
            xPosChecker++;
        }
        else if (xPosChecker == 3)
        {
            addPosX = -0.02f;
            xPosChecker--;
        }
        else
        {
            int choice = Random.Range(0, 2);
            if(choice == 0)
            {
                addPosX = 0.02f;
                xPosChecker++;
            }
            else
            {
                addPosX = -0.02f;
                xPosChecker--;
            }
        }
        

        while (transform.position.y < yMaxPos)
        {
            yield return new WaitForSeconds(DELAY);

            transform.position = new Vector2(transform.position.x + addPosX, transform.position.y + 0.05f);
        }

        while (transform.position.y > yStartPos)
        {
            yield return new WaitForSeconds(DELAY);

            transform.position = new Vector2(transform.position.x + addPosX, transform.position.y - 0.1f);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveKlovis());
    }

    private IEnumerator UpdatePhase()
    {
        yield return new WaitForSeconds(delayPhase);

        if (base.lifePoint < HEALTH_TO_PROC_PHASE_01 && phase == 0)
        {
            setPhase01();

            spawnParticleAtBottom(bubbleDropPref, true);
        }

        if(phase == 1)
        {
            delayPhase = Random.Range(2f, 3f);

            StartCoroutine(AnimateHead());
        }

        StartCoroutine(UpdatePhase());
    }

    private void spawnParticleAtBottom(GameObject _ptcPref, bool _mustDestroy)
    {
        GameObject ptc;
        ptc = Instantiate(_ptcPref, new Vector2(0f, -6.2f), Quaternion.identity);

        if(_mustDestroy) { Destroy(ptc, 21f); }
    }

    private void spawnTreasureCross()
    {
        crossTreasure = Instantiate(crossTreasurePref, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Destroy(crossTreasure);
        Destroy(bubbleCurtain);
    }

    private void setPhase01()
    {
        lockTreasure.SetActive(true);
        lockTreasureSprite.SetActive(false);
        lockTreasure.transform.localPosition = new Vector2(0f, 1.1f);

        phase = 1;
        base.soundManager.playAudioClipWithPitch(12, 2f);

        Rigidbody2D rbLock = lockTreasure.GetComponent<Rigidbody2D>();
        rbLock.gravityScale = 1f;
        Vector2 direction = lockTreasure.transform.position - base.target.position;
        rbLock.AddForce(direction * FORCE);
        rbLock.AddTorque(TORQUE);

        Destroy(lockTreasure, 5f);
    }

    private void chooseAttack()
    {
        soundManager.playAudioClip(18);

        int choice = Random.Range(0, 2);

        if(choice == 0) { throwJewels(); }
        else { throwCoins(); }
    }

    private void throwJewels()
    {
        float addAngle = Random.Range(0f, 30f);

        for(int i = 0; i < 360; i += 30)
        {
            throwJewelAtAngle(i + addAngle);
        }
    }

    private void throwCoins()
    {
        int coinsToThrow = 6;
        float limitAngle = 360 / coinsToThrow;

        for (int i = 0; i < coinsToThrow; i++)
        {
            float randomAngle = Random.Range(i * limitAngle, limitAngle + (i * limitAngle));

            throwCoinAtAngle(randomAngle);
        }
    }

    private void throwJewelAtAngle(float _addAngle)
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = Quaternion.identity;
        Quaternion quat;
        Vector2 dir;
        currentRot = new Vector3(0f, 0f, _addAngle);
        currentQuaternionRot.eulerAngles = currentRot;
        quat = currentQuaternionRot;

        GameObject jewel;
        jewel = Instantiate(jewelPref, transform.position, quat);
        Destroy(jewel, 4f);

        int selectedSprite = Random.Range(0, 3);
        jewel.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = jewelSprites[selectedSprite];

        dir = jewel.transform.up * SPEED_JEWEL;
        jewel.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
    }

    private void throwCoinAtAngle(float _addAngle)
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = Quaternion.identity;
        Quaternion quat;
        Vector2 dir;
        currentRot = new Vector3(0f, 0f, _addAngle);
        currentQuaternionRot.eulerAngles = currentRot;
        quat = currentQuaternionRot;

        GameObject coin;
        coin = Instantiate(piratCoinPref, transform.position, quat);

        float factorSpeed = Random.Range(1f, 2f);
        dir = coin.transform.up * (SPEED_JEWEL * factorSpeed);
        coin.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
    }
}
