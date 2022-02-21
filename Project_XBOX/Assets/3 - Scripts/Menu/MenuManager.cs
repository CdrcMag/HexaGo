using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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

    private bool isIntro = true;

    // =====================================================



    private void Update()
    {
        if((Input.GetButtonUp("Start") || Input.GetKeyUp(KeyCode.A)) && isIntro)
        {
            GoToLevelSelection();

            isIntro = false;
        }
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

        levelSelection.AnimateStart();
    }
}
