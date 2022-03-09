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
    [SerializeField] private Transform[] signs;
    private Transform[][] tentaclesComponents = new Transform[4][];
    private Transform[][] signsComponents = new Transform[4][];
    private Vector2[][] startPosSignsComponents = new Vector2[4][];

    private float magnitude = 2f;
    private float delayPhase = 5f;
    private int phase;

    private float rot = 0;
    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());

        SetTentaclesComponents();
        SetSignsComponents();
    }

    private void Start()
    {
        StartCoroutine(AnimateHead());
        StartCoroutine(AnimateBody());
        
        for(int i = 0; i < tentacles.Length; i++)
        {
            StartCoroutine(AnimateTentacle(i));
        }

        ResetSigns();
        StartCoroutine(UpdatePhase());
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

    private void SetSignsComponents()
    {
        for (int i = 0; i < signs.Length; i++)
        {
            signsComponents[i] = new Transform[4];
            startPosSignsComponents[i] = new Vector2[4];

            for(int j = 0; j < signsComponents.Length; j++)
            {
                signsComponents[i][j] = signs[i].GetChild(j);
                startPosSignsComponents[i][j] = signsComponents[i][j].position;
            }
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

    private IEnumerator UpdatePhase()
    {
        yield return new WaitForSeconds(delayPhase);

        phase = Random.Range(0, 1);

        if(phase == 0)
        {
            ResetSigns();
            StartCoroutine(ThrowTentacles());
            delayPhase = 12f;
        }

        StartCoroutine(UpdatePhase());
    }

    private IEnumerator ThrowTentacles()
    {
        int tentacle01 = Random.Range(0, 4);
        int tentacle02 = 0;

        while(tentacle01 == tentacle02)
        {
            tentacle02 = Random.Range(0, 4);
        }

        StartCoroutine(GlowSigns(tentacle01));
        StartCoroutine(GlowSigns(tentacle02));

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(ThrowTentacle(tentacle01));
        StartCoroutine(ThrowTentacle(tentacle02));
    }

    private IEnumerator ThrowTentacle(int _id)
    {
        if (_id == 0)
        {
            while (tentacles[_id].position.x < 0f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x + 0.1f, tentacles[_id].position.y);
            }
        }
        if (_id == 1)
        {
            while (tentacles[_id].position.x > 0f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x - 0.1f, tentacles[_id].position.y);
            }
        }
        else if (_id == 2 || _id == 3)
        {
            while (tentacles[_id].position.y > 0f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x, tentacles[_id].position.y - 0.1f);
            }
        }

        yield return new WaitForSeconds(3f);

        if (_id == 0)
        {
            while (tentacles[_id].position.x > -12f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x - 0.04f, tentacles[_id].position.y);
            }
        }
        if (_id == 1)
        {
            while (tentacles[_id].position.x < 12f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x + 0.04f, tentacles[_id].position.y);
            }
        }
        else if (_id == 2 || _id == 3)
        {
            while (tentacles[_id].position.y < 12f)
            {
                yield return new WaitForSeconds(0.01f);

                tentacles[_id].position = new Vector2(tentacles[_id].position.x, tentacles[_id].position.y + 0.04f);
            }
        }
    }

    private void ResetSigns()
    {
        for (int i = 0; i < signs.Length; i++)
        {
            for (int j = 0; j < signsComponents.Length; j++)
            {
                signsComponents[i][j].position = startPosSignsComponents[i][j];
                Color color = new Color(1f, 1f, 1f, 0f);

                SpriteRenderer spRd;
                spRd = signsComponents[i][j].GetComponent<SpriteRenderer>();
                spRd.color = color;
            }
        }
    }

    private IEnumerator GlowSigns(int _sign)
    {
        int cpt = 0;

        while(cpt < 4)
        {
            StartCoroutine(GlowSignComponent(_sign, cpt));
            StartCoroutine(TranslateSignComponent(_sign, cpt));

            yield return new WaitForSeconds(0.2f);

            cpt++;
        }

        cpt = 0;
        yield return new WaitForSeconds(1f);

        while (cpt < 4)
        {
            StartCoroutine(GlowSignComponent(_sign, cpt));

            yield return new WaitForSeconds(0.2f);

            cpt++;
        }
    }

    private IEnumerator GlowSignComponent(int _sign, int _component)
    {
        print("glow" + _sign + _component);
        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            SpriteRenderer spRd;
            spRd = signsComponents[_sign][_component].GetComponent<SpriteRenderer>();
            spRd.color = color;

            a -= 0.01f;

            yield return new WaitForSeconds(0.005f);
        }
    }

    private IEnumerator TranslateSignComponent(int _sign, int _component)
    {
        if(_sign == 0)
        {
            while (signsComponents[_sign][_component].position.x < startPosSignsComponents[_sign][_component].x + 2f)
            {
                yield return new WaitForSeconds(0.01f);

                signsComponents[_sign][_component].position = new Vector2(signsComponents[_sign][_component].position.x + 0.02f, signsComponents[_sign][_component].position.y);
            }
        }
        if (_sign == 1)
        {
            while (signsComponents[_sign][_component].position.x > startPosSignsComponents[_sign][_component].x - 2f)
            {
                yield return new WaitForSeconds(0.01f);

                signsComponents[_sign][_component].position = new Vector2(signsComponents[_sign][_component].position.x - 0.02f, signsComponents[_sign][_component].position.y);
            }
        }
        else if (_sign == 2 || _sign == 3)
        {
            while (signsComponents[_sign][_component].position.y > startPosSignsComponents[_sign][_component].y - 2f)
            {
                yield return new WaitForSeconds(0.01f);

                signsComponents[_sign][_component].position = new Vector2(signsComponents[_sign][_component].position.x, signsComponents[_sign][_component].position.y - 0.02f);
            }
        }
    }
}
