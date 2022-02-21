using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private SpriteRenderer selectALevel;
    [SerializeField] private Transform selecter;
    [SerializeField] private RectTransform[] levelIcons;

    private Image selecterImage;
    private bool canSelectLevel = false;

    // =====================================================4


    private void Awake()
    {
        selecterImage = selecter.GetComponent<Image>();

        selecter.gameObject.SetActive(false);

        for (int i = 0; i < levelIcons.Length; i++)
        {
            levelIcons[i].localScale = new Vector2(0f, 0f);
        }
    }

    private void Start()
    {
        StartCoroutine(AnimateSelectALevel());
        StartCoroutine(AnimateSelecter());
    }

    private IEnumerator AnimateSelectALevel()
    {
        float a = 1f;

        while (a > 0.6f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectALevel.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(DELAY);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectALevel.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(AnimateSelectALevel());
    }

    private IEnumerator AnimateSelecter()
    {
        float a = 1f;

        while (a > 0.6f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterImage.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(DELAY);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterImage.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(AnimateSelecter());
    }

    public void AnimateStart()
    {
        StartCoroutine(IAnimateStart());
    }

    private IEnumerator IAnimateStart()
    {
        yield return new WaitForSeconds(2f);

        int cpt = 0;

        while(cpt < levelIcons.Length)
        {
            while (levelIcons[cpt].localScale.x < 1)
            {
                levelIcons[cpt].localScale = new Vector2(levelIcons[cpt].localScale.x + 0.1f, levelIcons[cpt].localScale.y + 0.1f);

                yield return new WaitForSeconds(0.001f);
            }

            yield return new WaitForSeconds(0.001f);

            cpt++;
        }

        yield return new WaitForSeconds(0.5f);

        selecter.gameObject.SetActive(true);
        canSelectLevel = true;
    }
}
