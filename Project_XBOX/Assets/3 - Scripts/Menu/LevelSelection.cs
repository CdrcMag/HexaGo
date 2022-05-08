using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private const float DELAY = 0.01f;
    private const float ADD_SCALE = 0.03f;
    private const float MIN_ALPHA = 0.6f;
    private const float MAX_ALPHA = 1f;
    private const float MIN_SCALE = 0.75f;
    private const float MAX_SCALE = 1f;

    // ===================== VARIABLES =====================

    [SerializeField] private SpriteRenderer selectALevel;
    [SerializeField] private Transform selecter;
    [SerializeField] private Image selecterFlash;
    [SerializeField] private Transition transition;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private RectTransform[] difficultyButtons;
    [SerializeField] private GameObject[] visors;
    [SerializeField] private GameObject levelIconsRoot;
    [SerializeField] private SpriteRenderer selectADifficulty;

    private Image selecterImage;
    private bool canSelectLevel = false;
    private Coroutine animatedSelectADifficulty;

    // =====================================================4


    private void Awake()
    {
        selecterImage = selecter.GetComponent<Image>();

        canSelectLevel = true;
    }

    private void Start()
    {
        animatedSelectADifficulty = StartCoroutine(AnimateSelectADifficulty());
        StartCoroutine(AnimateSelectALevel());
        StartCoroutine(AnimateSelecter());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && canSelectLevel)
        {
            canSelectLevel = false;

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

        while (a > MIN_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectALevel.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(DELAY);
        }

        while (a < MAX_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectALevel.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(AnimateSelectALevel());
    }

    private IEnumerator AnimateSelectADifficulty()
    {
        float a = 1f;

        while (a > MIN_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectADifficulty.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(DELAY);
        }

        while (a < MAX_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selectADifficulty.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(DELAY);
        }

        animatedSelectADifficulty = StartCoroutine(AnimateSelectADifficulty());
    }

    private IEnumerator AnimateSelecter()
    {
        float a = 1f;

        while (a > MIN_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterImage.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(DELAY);
        }

        while (a < MAX_ALPHA)
        {
            Color color = new Color(1f, 1f, 1f, a);
            selecterImage.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(AnimateSelecter());
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
            soundManager.playAudioClip(2);
            return;
        }

        canSelectLevel = false;

        soundManager.playAudioClip(0);

        selecter.gameObject.SetActive(false);
        selecterFlash.gameObject.SetActive(true);

        StartCoroutine(IFlash(level));
    }

    public void SetDifficulty(int difficulty)
    {
        if (difficulty == 1) { PlayerPrefs.SetString("Difficulty", "Easy"); }
        else if (difficulty == 2) { PlayerPrefs.SetString("Difficulty", "Normal"); }
        else if (difficulty == 3) { PlayerPrefs.SetString("Difficulty", "Hard"); }

        CallScaleButton(difficulty);
        CallActivateVisor(difficulty);

        soundManager.playAudioClip(1);

        levelIconsRoot.SetActive(true);

        StopCoroutine(animatedSelectADifficulty);

        Color color = new Color(1f, 1f, 1f, MIN_ALPHA / 2);
        selectADifficulty.color = color;

        selectALevel.gameObject.SetActive(true);
    }

    private void CallScaleButton(int _id)
    {
        for(int i = 1; i <= 3; i++)
        {
            if(i == _id) { StartCoroutine(GrowButton(i - 1)); }
            else { StartCoroutine(SubstractButton(i - 1)); }
        }
    }

    private void CallActivateVisor(int _id)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i == _id) { visors[i - 1].SetActive(true); }
            else { visors[i - 1].SetActive(false); }
        }
    }

    private IEnumerator SubstractButton(int i)
    {
        while(difficultyButtons[i].localScale.x > MIN_SCALE)
        {
            yield return new WaitForSeconds(0.01f);
            difficultyButtons[i].localScale = new Vector2(difficultyButtons[i].localScale.x - ADD_SCALE, difficultyButtons[i].localScale.y - ADD_SCALE);
        }
    }

    private IEnumerator GrowButton(int i)
    {
        while (difficultyButtons[i].localScale.x < MAX_SCALE)
        {
            yield return new WaitForSeconds(0.01f);
            difficultyButtons[i].localScale = new Vector2(difficultyButtons[i].localScale.x + ADD_SCALE, difficultyButtons[i].localScale.y + ADD_SCALE);
        }
    }
}
