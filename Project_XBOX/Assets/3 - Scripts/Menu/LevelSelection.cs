using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private SpriteRenderer selectALevel;
    [SerializeField] private Transform selecter;
    [SerializeField] private RectTransform[] levelIcons;
    [SerializeField] private Image selecterFlash;
    [SerializeField] private Transition transition;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private RectTransform[] difficultyButtons;
    [SerializeField] private GameObject[] visors;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && canSelectLevel)
        {
            GoToLevelKeyboard();

            PlayerPrefs.SetString("Difficulty", "Easy");

            soundManager.playAudioClip(0);
        }
    }

    private void GoToLevelKeyboard()
    {
        selecter.gameObject.SetActive(false);
        selecterFlash.gameObject.SetActive(true);

        StartCoroutine(IFlash(1));
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
                levelIcons[cpt].localScale = new Vector2(levelIcons[cpt].localScale.x + 0.15f, levelIcons[cpt].localScale.y + 0.15f);

                yield return new WaitForSeconds(0.001f);
            }

            yield return new WaitForSeconds(0.001f);

            cpt++;
        }

        yield return new WaitForSeconds(0.2f);

        selecter.gameObject.SetActive(true);
        canSelectLevel = true;
    }

    private IEnumerator IFlash(float level)
    {
        float a = 1f;

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterFlash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        transition.StartAugment();

        yield return new WaitForSeconds(3f);

        if(level == 1)
        {
            SceneManager.LoadScene("Level01");
        }
    }

    public void GoToLevel(int level)
    {
        if(!canSelectLevel)
        {
            return;
        }

        if (level > PlayerPrefs.GetInt("LevelMax", 1))
        {
            return;
        }

        soundManager.playAudioClip(0);

        selecter.gameObject.SetActive(false);
        selecterFlash.gameObject.SetActive(true);

        StartCoroutine(IFlash(level));
    }

    public void SetDifficulty(int difficulty)
    {
        if(difficulty == 1)
        {
            PlayerPrefs.SetString("Difficulty", "Easy");
            print("difficulty is EASY");

            StartCoroutine(GrowButton(0));
            StartCoroutine(SubstractButton(1));
            StartCoroutine(SubstractButton(2));

            visors[0].SetActive(true);
            visors[1].SetActive(false);
            visors[2].SetActive(false);
        }
        else if (difficulty == 2)
        {
            PlayerPrefs.SetString("Difficulty", "Normal");
            print("difficulty is NORMAL");

            StartCoroutine(GrowButton(1));
            StartCoroutine(SubstractButton(0));
            StartCoroutine(SubstractButton(2));

            visors[0].SetActive(false);
            visors[1].SetActive(true);
            visors[2].SetActive(false);
        }
        else if (difficulty == 3)
        {
            PlayerPrefs.SetString("Difficulty", "Hard");
            print("difficulty is HARD");

            StartCoroutine(GrowButton(2));
            StartCoroutine(SubstractButton(1));
            StartCoroutine(SubstractButton(0));

            visors[0].SetActive(false);
            visors[1].SetActive(false);
            visors[2].SetActive(true);
        }
    }

    private IEnumerator SubstractButton(int i)
    {
        while(difficultyButtons[i].localScale.x > 0.75f)
        {
            yield return new WaitForSeconds(0.05f);
            difficultyButtons[i].localScale = new Vector2(difficultyButtons[i].localScale.x - 0.1f, difficultyButtons[i].localScale.y - 0.1f);
        }
    }

    private IEnumerator GrowButton(int i)
    {
        while (difficultyButtons[i].localScale.x < 1f)
        {
            yield return new WaitForSeconds(0.05f);
            difficultyButtons[i].localScale = new Vector2(difficultyButtons[i].localScale.x + 0.1f, difficultyButtons[i].localScale.y + 0.1f);
        }
    }
}
