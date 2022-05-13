using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private float MAX_HEALTHPOINT = 100f;
    private float ADD_SCALE = 0.025f;

    private Vector2 DIE_POS = new Vector2(100f, 100f);

    // ===================== VARIABLES =====================

    [Header("Objects in Scene")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Transition transition;
    [SerializeField] private SpriteRenderer[] bodies;
    [SerializeField] private RectTransform lifeBar;
    [SerializeField] private Transform borderRoot;

    [Header("Life Properties")]
    [SerializeField] private float lifePoint = 100;
    public float reduction = 0; // Pourcentage ( 0 - 1 )

    [Header("Candy")]
    [SerializeField] private GameObject[] candyPref;

    private bool isImmune = false;
    private SpriteRenderer[] borders = new SpriteRenderer[4];

    // =====================================================

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (borderRoot != null) { SetBorderSprites(); }
    }


    public void SetLifePoint(float _damage)
    {
        if (isImmune) { return; }

        float damageToTake = _damage - (_damage * reduction);

        lifePoint -= damageToTake;
        isImmune = true;

        CameraShake.Instance.Shake(0.2f, 1.2f);

        soundManager.playAudioClip(15);

        StartCoroutine(IUpdateLifeBarDown());
        StartCoroutine(IAnimationHit(bodies, true));
        if (borderRoot != null) { StartCoroutine(IAnimationHit(borders, false)); }

        if (lifePoint <= 0) { Die(); }
    }

    public float GetLifePoint() { return lifePoint; }

    private void Die()
    {
        soundManager.playAudioClip(7);

        PlayerPrefs.SetInt("Nbr_Morts", PlayerPrefs.GetInt("Nbr_Morts") + 1);

        transform.position = DIE_POS;

        StartCoroutine(ILoadMenu());
    }

    public IEnumerator ILoadMenu()
    {
        yield return new WaitForSeconds(1f);

        transition.StartAugment();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }

    private IEnumerator IAnimationHit(SpriteRenderer[] _spr, bool _isVisibleAtEnd)
    {
        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);

            for (int i = 0; i < _spr.Length; i++)
            {
                _spr[i].color = color;
            }

            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        isImmune = false;

        if (!_isVisibleAtEnd)
        {
            while (a > 0f)
            {
                Color color = new Color(1f, 1f, 1f, a);

                for (int i = 0; i < _spr.Length; i++)
                {
                    _spr[i].color = color;
                }

                a -= 0.1f;

                yield return new WaitForSeconds(0.005f);
            }
        }
    }

    private IEnumerator IUpdateLifeBarDown()
    {
        float scaleToReach = lifePoint / MAX_HEALTHPOINT;

        while (lifeBar.localScale.x > scaleToReach && lifeBar.localScale.x > 0f)
        {
            lifeBar.localScale = new Vector2(lifeBar.localScale.x - ADD_SCALE, lifeBar.localScale.y);

            yield return new WaitForSeconds(0.05f);
        }

        if (lifeBar.localScale.x < 0f)
        {
            lifeBar.localScale = new Vector2(0f, lifeBar.localScale.y);
        }
    }

    private IEnumerator IUpdateLifeBarUp()
    {
        float scaleToReach = lifePoint / MAX_HEALTHPOINT;

        while (lifeBar.localScale.x < scaleToReach && lifeBar.localScale.x > 0f)
        {
            lifeBar.localScale = new Vector2(lifeBar.localScale.x + ADD_SCALE, lifeBar.localScale.y);

            yield return new WaitForSeconds(0.05f);
        }

        if (lifeBar.localScale.x < 0f)
        {
            lifeBar.localScale = new Vector2(0f, lifeBar.localScale.y);
        }
    }

    public void ResetLifePoint()
    {
        lifePoint = MAX_HEALTHPOINT;
        lifeBar.localScale = new Vector2(1, 1);
    }

    public void AddHealthPoint(int points)
    {
        if (lifePoint + points >= MAX_HEALTHPOINT)
        {
            lifePoint = MAX_HEALTHPOINT;
        }
        else
        {
            lifePoint += points;
        }

        StartCoroutine(IUpdateLifeBarUp());
    }

    private void SetBorderSprites()
    {
        for (int i = 0; i < borderRoot.childCount; i++)
        {
            borders[i] = borderRoot.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    public void SpawnCandyAtPos(Vector2 _pos)
    {
        int choice = Random.Range(0, candyPref.Length);
        GameObject candy;
        candy = Instantiate(candyPref[choice], _pos, Quaternion.identity);
        soundManager.playAudioClip(23);
    }
}
