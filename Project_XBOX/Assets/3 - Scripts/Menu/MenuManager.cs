using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private GameObject pressToStart;
    [SerializeField] private GameObject pressToStartFlash;
    [SerializeField] private Transition transition;
    [SerializeField] private GameObject introMenu;
    [SerializeField] private GameObject levelSelectionMenu;
    [SerializeField] private GameObject levelSelectionMenuCanvas;
    [SerializeField] private LevelSelection levelSelection;
    [SerializeField] private SoundManager soundManager;

    private bool isIntro = false;
    private bool isReloading = false;

    // =====================================================


    private void Awake()
    {
        StartCoroutine(ILoadIntro());
    }

    private IEnumerator ILoadIntro()
    {
        yield return new WaitForSeconds(1.2f);

        isIntro = true;
    }

    private void Update()
    {
        if (isReloading)
            return;

        if((Input.GetButtonUp("Start") || Input.GetKeyUp(KeyCode.A)) && isIntro)
        {
            PlayerPrefs.SetInt("PlayedTutorial", 0);

            GoToLevelSelection();

            soundManager.playAudioClip(0);

            isIntro = false;
        }

        if(Input.GetButtonDown("Xbox_B"))
        {
            isReloading = true;
            transition.Augment();
            StartCoroutine(WaitAndReload());
        }
    }

    IEnumerator WaitAndReload()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }



    private void GoToLevelSelection()
    {
        pressToStart.SetActive(false);
        pressToStartFlash.SetActive(true);

        StartCoroutine(IFlash());
    }

    private IEnumerator IFlash()
    {
        float a = 1f;
        SpriteRenderer flash = pressToStartFlash.GetComponent<SpriteRenderer>();

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            flash.color = color;
            a += 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        transition.StartFullTransition();

        yield return new WaitForSeconds(2f);

        introMenu.SetActive(false);
        levelSelectionMenu.SetActive(true);
        levelSelectionMenuCanvas.SetActive(true);
    }
}
