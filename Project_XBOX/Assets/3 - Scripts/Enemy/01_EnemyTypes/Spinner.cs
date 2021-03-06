using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Enemy
{
    private const float MIN_DELAY_CHARGE = 7f;
    private const float MAX_DELAY_CHARGE = 12f;
    private const float MAX_ROTATINGSPEED = -1000f;
    private const float LESS_ROTATINGSPEED = 20f;
    private const float MAX_SPEED = 4f;

    // ===================== VARIABLES =====================

    private float rotatingSpeed = -200f;
    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform spin;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());

        StartCoroutine(Charge());
    }

    public void Update()
    {
        base.MoveToward();

        RotateSpin(rotatingSpeed);
    }
    
    // Rotate the spin
    private void RotateSpin(float _speed)
    {
        spin.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    // Charge animation
    private IEnumerator Charge()
    {
        float delay = Random.Range(MIN_DELAY_CHARGE, MAX_DELAY_CHARGE);

        yield return new WaitForSeconds(delay);

        SetSpeed(0f);
        GetEye(0).MakeAngryEye();

        while(rotatingSpeed > MAX_ROTATINGSPEED)
        {
            yield return new WaitForSeconds(0.01f);

            rotatingSpeed -= LESS_ROTATINGSPEED;
        }

        SetSpeed(MAX_SPEED);

        while (rotatingSpeed < -200)
        {
            yield return new WaitForSeconds(0.01f);

            rotatingSpeed += LESS_ROTATINGSPEED / 2;

            SetSpeed(GetSpeed() - 0.025f);
        }

        StartCoroutine(Charge());
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        if(!isGrowingBody)
            StartCoroutine(GrowBody());
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1.3f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + 0.04f, body.localScale.y + 0.04f);
        }

        while (body.localScale.x > 1f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - 0.04f, body.localScale.y - 0.04f);
        }

        isGrowingBody = false;
    }
}