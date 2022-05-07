using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poulpy : Enemy
{
    private const float DELAY = 0.01f;
    private const float ADDSCALE = 0.05f;

    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    [SerializeField] private Transform[] tentacles;
    [SerializeField] private Transform[] signs;
    [SerializeField] private SpriteRenderer[] signsMine;
    [SerializeField] private GameObject minePref;
    [SerializeField] private GameObject anchor;
    [SerializeField] private SpriteRenderer[] signsAnchor;
    [SerializeField] private Transform[] posAnchor;
    [SerializeField] private GameObject angryPoulpy;
    [SerializeField] private GameObject headbandPref;

    [Header("Particles")]
    [SerializeField] private GameObject bubbleCurtainPref;
    [SerializeField] private GameObject bubbleDropPref;

    private Transform[][] tentaclesComponents = new Transform[4][];
    private Transform[][] signsComponents = new Transform[4][];
    private Vector2[][] startPosSignsComponents = new Vector2[4][];
    private bool[] spotMineChecked = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

    private float magnitude = 2f;
    private float delayPhase = 7f;
    private int phase;
    private int maxPhase = 1;
    private bool hasSummon = false;

    private float rot = 0;
    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
        base.SetMaxLifePoint();

        SetTentaclesComponents();
        SetSignsComponents();

        StartCoroutine(IAnimateHandband());

        spawnParticleAtBottom(bubbleCurtainPref, false);
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

        StartCoroutine(IPlayBellSound());
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

        for(int k = 0; k < signsMine.Length; k++)
        {
            Color color = new Color(1f, 1f, 1f, 0f);
            signsMine[k].color = color;
        }

        for (int l = 0; l < signsAnchor.Length; l++)
        {
            Color color = new Color(1f, 1f, 1f, 0f);
            signsAnchor[l].color = color;
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

        spawnParticleAtBottom(bubbleDropPref, true);

        if (maxPhase == 1)
        {
            phase = 0;
        }
        else
        {
            int ratePhase = Random.Range(1, 101);
            if(ratePhase < 35) { phase = 0; }
            else { phase = 1; }
        }
        //phase = Random.Range(0, maxPhase);

        if(maxPhase == 3)
        {
            delayPhase = 10f;
            maxPhase = 2;

            base.cameraShake.callPoulpyShake();
            base.soundManager.playAudioClipWithPitch(12, 2f);

            angryPoulpy.SetActive(true);

            for(int i = 0; i < signsAnchor.Length; i++)
            {
                StartCoroutine(AlertAnchor(i));
            }

            yield return new WaitForSeconds(2f);

            for (int j = 0; j < posAnchor.Length; j++)
            {
                StartCoroutine(SpawnAnchor(posAnchor[j]));
            }
        }
        else if (phase == 0 && maxPhase != 3)
        {
            ResetSigns();
            StartCoroutine(ThrowTentacles());
            delayPhase = 12f;
        }
        else if(phase == 1 && maxPhase != 3)
        {
            int pattern = Random.Range(0, 3);

            if(pattern == 0)
            {
                delayPhase = 7f;
                StartCoroutine(Pattern01());
            }
            else if (pattern == 1)
            {
                delayPhase = 4f;
                StartCoroutine(Pattern02());
            }
            else if (pattern == 2)
            {
                delayPhase = 7f;
                StartCoroutine(Pattern03());
            }
        }

        UnlockPhase();

        StartCoroutine(UpdatePhase());
    }

    private void UnlockPhase()
    {
        if(lifePoint < 1500f && maxPhase == 1)
        {
            maxPhase = 2;
        }

        if(lifePoint < 700f && !hasSummon && maxPhase == 2)
        {
            hasSummon = true;
            maxPhase = 3;
        }
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
            //StartCoroutine(TranslateSignComponent(_sign, cpt));

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

    private IEnumerator AlertAnchor(int _id)
    {
        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            signsAnchor[_id].color = color;

            a -= 0.04f;

            yield return new WaitForSeconds(0.01f);
        }

        a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            signsAnchor[_id].color = color;

            a -= 0.04f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator SpawnAnchor(Transform _t)
    {
        GameObject enemy = Instantiate(anchor, _t.position, Quaternion.identity);

        enemy.transform.localScale = new Vector2(0f, 0f);

        while (enemy != null && enemy.transform.localScale.x < 1)
        {
            yield return new WaitForSeconds(DELAY);

            if (enemy != null)
                enemy.transform.localScale = new Vector2(enemy.transform.localScale.x + ADDSCALE, enemy.transform.localScale.y + ADDSCALE);
        }
    }

    private IEnumerator Pattern01()
    {
        ResetSpotMineChecked();

        int cpt01 = 0;
        int cpt02 = 0;

        while(cpt01 < 5)
        {
            while(cpt02 < 2)
            {
                StartCoroutine(AlertMine(ReturnValidSpot(), 1));

                cpt02++;
            }

            cpt01++;
            cpt02 = 0;

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Pattern02()
    {
        StartCoroutine(AlertMine(0, 0.8f));
        StartCoroutine(AlertMine(1, 0.8f));
        StartCoroutine(AlertMine(4, 0.8f));
        StartCoroutine(AlertMine(5, 0.8f));
        StartCoroutine(AlertMine(9, 0.8f));
        StartCoroutine(AlertMine(10, 0.8f));
        StartCoroutine(AlertMine(13, 0.8f));
        StartCoroutine(AlertMine(14, 0.8f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(AlertMine(2, 0.8f));
        StartCoroutine(AlertMine(3, 0.8f));
        StartCoroutine(AlertMine(6, 0.8f));
        StartCoroutine(AlertMine(7, 0.8f));
        StartCoroutine(AlertMine(11, 0.8f));
        StartCoroutine(AlertMine(12, 0.8f));
        StartCoroutine(AlertMine(15, 0.8f));
        StartCoroutine(AlertMine(16, 0.8f));
    }

    private IEnumerator Pattern03()
    {
        for(int i = 0; i < Mathf.Floor(signsMine.Length / 2) - 1; i++)
        {
            StartCoroutine(AlertMine(i, 1f));
            StartCoroutine(AlertMine(signsMine.Length - 1 - i, 1f));

            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(AlertMine(8, 2f));
    }

    private void ResetSpotMineChecked()
    {
        for(int i = 0; i < spotMineChecked.Length; i++)
        {
            spotMineChecked[i] = false;
        }
    }

    private int ReturnValidSpot()
    {
        int idSpot = 0;

        while (spotMineChecked[idSpot])
        {
            idSpot = Random.Range(0, spotMineChecked.Length);
        }

        spotMineChecked[idSpot] = true;

        return idSpot;
    }

    private IEnumerator AlertMine(int _id, float _scale)
    {
        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            signsMine[_id].color = color;

            a -= 0.04f;

            yield return new WaitForSeconds(0.01f);
        }

        a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            signsMine[_id].color = color;

            a -= 0.04f;

            yield return new WaitForSeconds(0.01f);
        }
        ShotMine(_id, _scale);
    }

    private void ShotMine(int _id, float _scale)
    {
        GameObject mine;
        mine = Instantiate(minePref, signsMine[_id].transform.parent.position, Quaternion.identity);
        mine.transform.localScale = new Vector2(_scale, _scale);
        Vector2 dir = mine.transform.up * 15f;
        mine.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
    }

    private IEnumerator IPlayBellSound()
    {
        yield return new WaitForSeconds(2f);

        base.soundManager.playAudioClipWithPitch(12, 2f);
    }

    private void spawnParticleAtBottom(GameObject _ptcPref, bool _mustDestroy)
    {
        GameObject ptc;
        ptc = Instantiate(_ptcPref, new Vector2(0f, -6.2f), Quaternion.identity);

        if(_mustDestroy) { Destroy(ptc, 21f); }
    }

    private IEnumerator IAnimateHandband()
    {
        GameObject headband;
        headband = Instantiate(headbandPref, new Vector2(0f, 3.2f), Quaternion.identity);
        SpriteRenderer sprHeadband = headband.GetComponent<SpriteRenderer>();
        SpriteRenderer sprName = headband.transform.GetChild(0).GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(3f);

        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprName.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprName.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprName.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprName.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprName.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        Color colorEnd = new Color(1f, 1f, 1f, 0);
        sprName.color = colorEnd;

        yield return new WaitForSeconds(1.5f);
        a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            sprHeadband.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.05f);
        }

        sprHeadband.color = colorEnd;

        yield return new WaitForSeconds(1.5f);

        Destroy(headband);
    }
}
