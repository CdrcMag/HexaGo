using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klovis : Enemy
{
    private const float DELAY = 0.01f;
    private const float ADDSCALE = 0.05f;
    private const float FORCE = 100f;
    private const float TORQUE = 20f;

    private const float HEALTH_TO_PROC_PHASE_01 = 600f;

    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    [SerializeField] private GameObject lockTreasure;

    [Header("Prefab")]
    [SerializeField] private GameObject crossTreasurePref;

    [Header("Particles")]
    [SerializeField] private GameObject bubbleCurtainPref;
    [SerializeField] private GameObject bubbleDropPref;

    private float delayPhase = 1f;
    private int phase = 0;
    private int state = 0;

    private GameObject crossTreasure;

    private float rot = 0;
    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
        base.SetMaxLifePoint();

        spawnParticleAtBottom(bubbleCurtainPref, false);
    }

    private void Start()
    {
        spawnTreasureCross();
        lockTreasure.transform.localPosition = new Vector2(0f, 1.1f);

        StartCoroutine(UpdatePhase());
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    private IEnumerator AnimateHead()
    {
        while(head.localPosition.y > -0.7f)
        {
            yield return new WaitForSeconds(DELAY);

            head.localPosition = new Vector2(head.localPosition.x, head.localPosition.y - 0.04f);
        }

        yield return new WaitForSeconds(0.5f);

        while (head.localPosition.y < 0)
        {
            yield return new WaitForSeconds(DELAY);

            head.localPosition = new Vector2(head.localPosition.x, head.localPosition.y + 0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(AnimateHead());
    }

    private IEnumerator AnimateBody()
    {
        yield return new WaitForSeconds(0.5f);

        while (body.localPosition.y > -3.9f)
        {
            yield return new WaitForSeconds(DELAY);

            body.localPosition = new Vector2(body.localPosition.x, body.localPosition.y - 0.04f);
        }

        yield return new WaitForSeconds(0.1f);

        while (body.localPosition.y < -3.2f)
        {
            yield return new WaitForSeconds(DELAY);

            body.localPosition = new Vector2(body.localPosition.x, body.localPosition.y + 0.02f);
        }

        yield return new WaitForSeconds(0.4f);

        StartCoroutine(AnimateBody());
    }

    private IEnumerator UpdatePhase()
    {
        yield return new WaitForSeconds(delayPhase);

        if (base.lifePoint < HEALTH_TO_PROC_PHASE_01 && phase == 0)
        {
            setPhase01();

            spawnParticleAtBottom(bubbleDropPref, true);

            delayPhase = 2f;
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
    }

    private void setPhase01()
    {
        phase = 1;
        base.soundManager.playAudioClipWithPitch(12, 2f);

        Rigidbody2D rbLock = lockTreasure.GetComponent<Rigidbody2D>();
        rbLock.gravityScale = 1f;
        Vector2 direction = lockTreasure.transform.position - base.target.position;
        rbLock.AddForce(direction * FORCE);
        rbLock.AddTorque(TORQUE);

        Destroy(lockTreasure, 5f);
    }
}
