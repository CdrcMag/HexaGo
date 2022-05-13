using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_System : MonoBehaviour
{
    public static Pause_System Instance = null;

    private bool OnPause = false;//Not on pause
    private bool inputBlocked = false;

    [HideInInspector] public GameObject PauseMenu;
    [HideInInspector] public bool canPause = true;
    [HideInInspector] public float TimePlayed = 0;
    [HideInInspector] public GameObject PauseMenuChild;

    public int ButtonSelected = 1;

    public GameObject[] buttons;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        PauseMenu = GameObject.Find("Pause").GetComponent<RectTransform>().GetChild(0).gameObject;

        SetPauseOff();

        StartCoroutine(LikeAnUpdate());
    }

    IEnumerator LikeAnUpdate()
    {
        while(true)
        {
            if ((Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape)) && canPause)
            {
                if (OnPause) SetPauseOff();
                else if (!OnPause) SetPauseOn();
            }

            yield return null;

            
        }
    }

    private void Update()
    {
        TimePlayed += Time.deltaTime;

        HandleControllerInputs();

    }

    public void SetPauseOn()
    {
        Camera.main.GetComponent<CameraShake>().StopShaking();
        PlayerPrefs.SetFloat("TimePlayed", PlayerPrefs.GetFloat("TimePlayed") + TimePlayed);
        TimePlayed = 0;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        StartCoroutine(WaitFor());
        OnPause = true;
        
    }

    IEnumerator WaitFor()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        PauseMenuChild = PauseMenu.transform.GetChild(1).gameObject;
        PauseMenuChild.GetComponent<Recap>().GenerateIcons();

    }

    public void SetPauseOff()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        OnPause = false;
    }

    public bool GetPauseState()
    {
        return OnPause;
    }

    public void InputButtonPauseMenu(string s)
    {
        switch(s)
        {
            case "Reprendre":
                SetPauseOff();
                break;
            case "Options":
                break;
            case "Quitter":
                break;
            default:
                break;

        }
    }


    private void HandleControllerInputs()
    {
        if (Input.GetButtonDown("Xbox_Validation") && OnPause && ButtonSelected == 1)
        {
            SetPauseOff();
            StartCoroutine(BlockInput(.15f));
        }

        //Quitte le menu de pause
        if (Input.GetButtonDown("Xbox_B") && OnPause)
        {
            SetPauseOff();
            StartCoroutine(BlockInput(.15f));
        }

        //Monte dans la liste des boutons
        if (Input.GetAxisRaw("Xbox_Vertical") == 1 && !inputBlocked && ButtonSelected - 1 > 0)
        {
            StartCoroutine(BlockInput(.15f));
            ButtonSelected--;
        }
        //Descends dans la liste des boutons
        if (Input.GetAxisRaw("Xbox_Vertical") == -1 && !inputBlocked && ButtonSelected + 1 < 4)
        {
            StartCoroutine(BlockInput(.15f));
            ButtonSelected++;
        }

        //Ajoute le feedback sur les boutons
        SetFeedbackOnButton();

    }

    private void SetFeedbackOnButton()
    {
        for (int i = 0; i < 3; i++)
        {
            buttons[i].transform.localPosition = new Vector2(-32, buttons[i].transform.localPosition.y);
            buttons[i].transform.localScale = new Vector2(1, 1);
        }

        buttons[ButtonSelected - 1].transform.localPosition = new Vector2(-16, buttons[ButtonSelected - 1].transform.localPosition.y);
        buttons[ButtonSelected - 1].transform.localScale = new Vector2(1.2f, 1.2f);
    }


    IEnumerator BlockInput(float s)
    {
        inputBlocked = true;
        yield return new WaitForSecondsRealtime(s);
        inputBlocked = false;
    }
}
