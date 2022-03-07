using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private float lifePoint = 100;
    public float reduction = 0;//Percentage ( 0 - 1 )
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Transition transition;
    [SerializeField] private SpriteRenderer[] bodies;
    private bool isImmune = false;

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _damage) 
    {
        if(isImmune)
        {
            return;
        }

        float damageToTake = _damage - (_damage * reduction);

        lifePoint -= damageToTake;
        isImmune = true;

        StartCoroutine(AnimationHit());
        
        if(lifePoint <= 0)
        {
            Die();
        }
    }

    public float GetLifePoint() { return lifePoint; }

    // =====================================================


    private void Die()
    {
        soundManager.playAudioClip(7);
        transform.position = new Vector2(100f, 100f);

        StartCoroutine(ILoadMenu());
    }

    private IEnumerator ILoadMenu()
    {
        yield return new WaitForSeconds(1f);

        transition.StartAugment();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }

    private IEnumerator AnimationHit()
    {
        float a = 1f;

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for(int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        isImmune = false;
    }
}
