using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCrater : MonoBehaviour
{
    public SpriteRenderer spr;


    private void Start()
    {
        StartCoroutine(IFade());
    }

    private IEnumerator IFade()
    { 
        yield return new WaitForSeconds(1.2f);

        Color color;
        float a = 1f;

        while (a > 0f)
        {
            color = new Color(1f, 1f, 1f, a);
            spr.color = color;

            a -= 0.1f;

            yield return new WaitForSeconds(0.05f);
        }

        color = new Color(1f, 1f, 1f, 0f);
        spr.color = color;
    }
}
