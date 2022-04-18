using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{
    // ===================== VARIABLES =====================

    private float rotatingSpeed = 100f;
    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform gear;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    public void Update()
    {
        base.MoveToward();

        RotateGear(rotatingSpeed);
    }

    // Rotate the spin
    private void RotateGear(float _speed)
    {
        gear.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        if (!isGrowingBody)
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
