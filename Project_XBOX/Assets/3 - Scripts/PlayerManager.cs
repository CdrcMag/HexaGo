using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    // ===================== VARIABLES =====================

    [SerializeField] private float lifePoint = 100;
    public float reduction = 0;//Percentage ( 0 - 1 )
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Transition transition;
    [SerializeField] private SpriteRenderer[] bodies;
    private bool isImmune = false;
    [SerializeField] private RectTransform lifeBar;

    public int ChanceToSpawnHealthPotion = 50;//Pourcentage
    public int HealAmount = 15;//Pourcentage

    // =====================================================

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _damage) 
    {
        if(isImmune)
        {
            return;
        }

        float damageToTake = _damage - (_damage * reduction);

        CameraShake.Instance.Shake(0.2f, 1.2f);

        lifePoint -= damageToTake;
        isImmune = true;

        StartCoroutine(AnimationHit());
        StartCoroutine(UpdateLifeBar());
        
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

    private IEnumerator UpdateLifeBar()
    {
        float scaleToReach = lifePoint / 100;

        while(lifeBar.localScale.x > scaleToReach && lifeBar.localScale.x > 0f)
        {
            lifeBar.localScale = new Vector2(lifeBar.localScale.x - 0.025f, lifeBar.localScale.y);

            yield return new WaitForSeconds(0.05f);
        }

        if(lifeBar.localScale.x < 0f)
        {
            lifeBar.localScale = new Vector2(0f, lifeBar.localScale.y);
        }
    }

    private IEnumerator UpdateLifeBarUp()
    {
        float scaleToReach = lifePoint / 100;

        while (lifeBar.localScale.x < scaleToReach && lifeBar.localScale.x > 0f)
        {
            lifeBar.localScale = new Vector2(lifeBar.localScale.x + 0.025f, lifeBar.localScale.y);

            yield return new WaitForSeconds(0.05f);
        }

        if (lifeBar.localScale.x < 0f)
        {
            lifeBar.localScale = new Vector2(0f, lifeBar.localScale.y);
        }
    }

    public void ResetLifePoint()
    {
        lifePoint = 100;
        lifeBar.localScale = new Vector2(1, 1);
    }

    public void AddHealthPoint(int points)
    {
        if(lifePoint + points >= 100)
        {
            lifePoint = 100;
        }
        else
        {
            lifePoint += points;
        }

        StartCoroutine(UpdateLifeBarUp());
    }
}
