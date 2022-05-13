using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : Enemy
{
    private const float SHAKE_DELAY_ACTIVATION = 4f;
    private const float ADDSCALE = 0.04f;
    private const float DELAY_SHAKE = 0.003f;

    // ===================== VARIABLES =====================

    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());

        StartCoroutine(IAnimShake());
    }

    public override void TakeDamage(float _damage, bool _b)
    {
        base.TakeDamage(_damage);
        if (!isGrowingBody) { StartCoroutine(GrowBody()); }
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.Instance.AddHealthPoint(5);
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + ADDSCALE, body.localScale.y + ADDSCALE);
        }

        while (body.localScale.x > 0.6f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - ADDSCALE, body.localScale.y - ADDSCALE);
        }

        isGrowingBody = false;
    }

    private IEnumerator IAnimShake()
    {
        yield return new WaitForSeconds(SHAKE_DELAY_ACTIVATION);

        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();
        int cpt = 0;

        while (cpt < 11)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        cpt = 0;

        while (cpt < 22)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z - 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        cpt = 0;

        while (cpt < 11)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        StartCoroutine(IAnimShake());
    }
}
