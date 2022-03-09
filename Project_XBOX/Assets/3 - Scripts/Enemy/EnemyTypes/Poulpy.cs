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
    [SerializeField] private Transform[] tentacles;
    private Transform[][] tentaclesComponents = new Transform[4][];

    private const float magnitude = 2f;

    private float rot = 0;
    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());

        SetTentaclesComponents();
    }

    private void Start()
    {
        StartCoroutine(AnimateHead());
        StartCoroutine(AnimateBody());
        
        for(int i = 0; i < tentacles.Length; i++)
        {
            StartCoroutine(AnimateTentacle(i));
        }
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

    private void SetTentaclesComponents()
    {
        for(int i = 0; i < tentacles.Length; i++)
        {
            tentaclesComponents[i] = new Transform[6];

            tentaclesComponents[i][0] = tentacles[i].GetChild(0);
            tentaclesComponents[i][1] = tentaclesComponents[i][0].GetChild(1);
            tentaclesComponents[i][2] = tentaclesComponents[i][1].GetChild(1);
            tentaclesComponents[i][3] = tentaclesComponents[i][2].GetChild(1);
            tentaclesComponents[i][4] = tentaclesComponents[i][3].GetChild(1);
            tentaclesComponents[i][5] = tentaclesComponents[i][4].GetChild(1);
        }
    }

    private IEnumerator AnimateTentacle(int _id)
    {
        while(rot < magnitude)
        {
            rot += 0.1f;
            currentRot = new Vector3(0, 0, rot);
            currentQuaternionRot.eulerAngles = currentRot;

            for(int i = 0; i < 6; i++)
            {
                tentaclesComponents[_id][i].localRotation = currentQuaternionRot;
            }

            yield return new WaitForSeconds(0.01f);
        }

        while (rot > magnitude * -1)
        {
            rot = rot - 0.15f;
            currentRot = new Vector3(0, 0, rot);
            currentQuaternionRot.eulerAngles = currentRot;

            for (int i = 0; i < 6; i++)
            {
                tentaclesComponents[_id][i].localRotation = currentQuaternionRot;
            }

            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(AnimateTentacle(_id));
    }
}
