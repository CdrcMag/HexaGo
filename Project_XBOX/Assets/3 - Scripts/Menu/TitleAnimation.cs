using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TitleAnimation : MonoBehaviour
{
    private const float MININTENSITY = 0.5f;
    private const float MAXINTENSITY = 1.2f;
    private const float MINSCALE = 1.5f;
    private const float MAXSCALE = 2f;
    private const float DELAY = 0.01f;
    private const float ADDINTENSITY = 0.04f;

    // ===================== VARIABLES =====================

    [SerializeField] private Transform[] letters;
    [SerializeField] private Light2D[] lights;
    [SerializeField] private SpriteRenderer pressToStart;
    [SerializeField] private Light2D[] lightsLaser;

    // =====================================================


    private void Start()
    {
        StartCoroutine(AnimateLights());
        StartCoroutine(AnimateLetters());
        StartCoroutine(AnimatePress());
        StartCoroutine(AnimateLightsLaser());
    }

    private IEnumerator AnimateLights()
    {
        while(lights[0].intensity < MAXINTENSITY)
        {
            for(int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity += ADDINTENSITY;
                yield return new WaitForSeconds(DELAY);
            }
        }

        yield return new WaitForSeconds(0.2f);

        while (lights[0].intensity > MININTENSITY)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity -= ADDINTENSITY;
                yield return new WaitForSeconds(DELAY);
            }
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(AnimateLights());
    }

    private IEnumerator AnimateLetters()
    {
        int cpt = 0;

        while(cpt < letters.Length)
        {
            StartCoroutine(GrowLetter(letters[cpt]));

            yield return new WaitForSeconds(0.2f);

            cpt++;
        }

        yield return new WaitForSeconds(4f);

        StartCoroutine(AnimateLetters());
    }

    private IEnumerator GrowLetter(Transform _letter)
    {
        while (_letter.localScale.x < MAXSCALE)
        {
            _letter.localScale = new Vector2(_letter.localScale.x + 0.04f, _letter.localScale.y + 0.04f);

            yield return new WaitForSeconds(0.005f);
        }

        while (_letter.localScale.x > MINSCALE)
        {
            _letter.localScale = new Vector2(_letter.localScale.x - 0.03f, _letter.localScale.y - 0.03f);

            yield return new WaitForSeconds(0.005f);
        }
    }

    private IEnumerator AnimatePress()
    {
        float a = 1f;

        while(a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            pressToStart.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            pressToStart.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(AnimatePress());
    }

    private IEnumerator AnimateLightsLaser()
    {
        while (lightsLaser[0].intensity < 1.2f)
        {
            for (int i = 0; i < lightsLaser.Length; i++)
            {
                lightsLaser[i].intensity += ADDINTENSITY;
                yield return new WaitForSeconds(DELAY);
            }
        }

        yield return new WaitForSeconds(0.2f);

        while (lightsLaser[0].intensity > 0.6f)
        {
            for (int i = 0; i < lightsLaser.Length; i++)
            {
                lightsLaser[i].intensity -= ADDINTENSITY;
                yield return new WaitForSeconds(DELAY);
            }
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(AnimateLightsLaser());
    }
}
