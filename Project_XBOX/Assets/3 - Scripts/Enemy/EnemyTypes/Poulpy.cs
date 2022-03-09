using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poulpy : Enemy
{
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    private void Start()
    {
        StartCoroutine(AnimateHead());
        StartCoroutine(AnimateBody());
    }

    private void Update()
    {

    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    private IEnumerator AnimateHead()
    {
        while(head.localPosition.y > -0.8f)
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

        while (body.localPosition.y > -4f)
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
}
