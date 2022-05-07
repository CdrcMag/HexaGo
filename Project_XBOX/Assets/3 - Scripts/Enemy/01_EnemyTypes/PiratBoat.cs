using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratBoat : Enemy
{
    // ===================== VARIABLES =====================

    [Header("Boat Properties")]
    [SerializeField] private float boatSpeed = 4f;
    [SerializeField] private float endPosY = 0f;

    [Header("Boat Component")]
    [SerializeField] private Transform foamRoot;
    [SerializeField] private SpriteRenderer[] signComponents;
    [SerializeField] private GameObject[] canons;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
    }

    private void Start()
    {
        StartCoroutine(IGlowSigns());
        StartCoroutine(IMoveForward());
    }

    private IEnumerator IMoveForward()
    {
        yield return new WaitForSeconds(4f);

        base.soundManager.playAudioClip(13);
        float endPosX = transform.position.x;
        Vector2 target = new Vector2(endPosX, endPosY);

        for(int i = 0; i < canons.Length; i++)
        {
            canons[i].SetActive(true);
        }
 
        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, boatSpeed * Time.deltaTime);

            yield return null;
        }

        for(int i = 0; i < foamRoot.childCount; i++)
        {
            foamRoot.GetChild(i).GetComponent<ParticleSystem>().Stop();
        }
    }

    private IEnumerator IGlowSigns()
    {
        int cpt = 0;

        while (cpt < 4)
        {
            StartCoroutine(IGlowSignComponent(cpt));

            yield return new WaitForSeconds(0.2f);

            cpt++;
        }

        cpt = 0;
        yield return new WaitForSeconds(1f);

        while (cpt < 4)
        {
            StartCoroutine(IGlowSignComponent(cpt));

            yield return new WaitForSeconds(0.2f);

            cpt++;
        }
    }

    private IEnumerator IGlowSignComponent(int _component)
    {
        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            SpriteRenderer spRd;
            spRd = signComponents[_component].GetComponent<SpriteRenderer>();
            spRd.color = color;

            a -= 0.01f;

            yield return new WaitForSeconds(0.005f);
        }
    }
}
