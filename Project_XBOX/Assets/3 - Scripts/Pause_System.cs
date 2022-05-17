using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause_System : MonoBehaviour
{
    public static Pause_System Instance = null;

    public TextMeshProUGUI volumeText;
    public RectTransform boule;
    private AudioSource[] sources = new AudioSource[0];

    [HideInInspector] public GameObject PauseMenu;
    [HideInInspector] public bool canPause = true;
    [HideInInspector] public float TimePlayed = 0;
    [HideInInspector] public GameObject PauseMenuChild;

    public int ButtonSelected = 1;

    public GameObject[] buttons;
    public GameObject pauseConfirmation;
    public GameObject MenuOptions;
    public GameObject MenuArmes;
    public GameObject MenuStats;

    private bool OnPause = false;//Not on pause
    private bool inputBlocked = false;
    private bool inMenu = false;
    private bool inConfirmation = false;
    private bool inOptions = false;
    private bool inVolume = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        PauseMenu = GameObject.Find("Pause").GetComponent<RectTransform>().GetChild(0).gameObject;

        sources = GameObject.FindObjectsOfType<AudioSource>();

        SetPauseOff();

        StartCoroutine(LikeAnUpdate());

        MenuOptions.SetActive(false);
    }

    IEnumerator LikeAnUpdate()
    {
        while(true)
        {
            if ((Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape)) && canPause)
            {
                if (!OnPause) SetPauseOn();
            }

            yield return null;

            
        }
    }

    private void Update()
    {
        TimePlayed += Time.deltaTime;

        if (inMenu) HandleControllerInputs();
        else if (inConfirmation) HandleControllerInputsConfirmation();
        else if (inOptions) HandleControllerInputsOptions();
    }

    public void SetPauseOn()
    {
        inMenu = true;
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
        ButtonSelected = 1;
        confirmationPosition = 1;
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
                SetOptionsOn();
                break;
            case "Quitter":
                QuitFirstStep();
                break;
            default:
                break;

        }
    }

    private void SetOptionsOn()
    {
        inMenu = false;
        inOptions = true;
        MenuArmes.SetActive(false);
        MenuStats.SetActive(false);
        MenuOptions.SetActive(true);
        HandleMines(false);
        inVolume = true;
    }
    private void SetOptionsOff()
    {
        inMenu = true;
        inOptions = false;
        MenuArmes.SetActive(true);
        MenuStats.SetActive(true);
        MenuOptions.SetActive(false);
        HandleMines(true);
        inVolume = false;

    }

    private void QuitFirstStep()
    {
        inConfirmation = true;
        inMenu = false;
        pauseConfirmation.SetActive(true);
    }
    private void ReverseFirstStep()
    {
        inConfirmation = false;
        inMenu = true;
        confirmationPosition = 1;
        pauseConfirmation.SetActive(false);
    }

    private void HandleMines(bool state)
    {
        GameObject pool = GameObject.Find("EnemyPool");

        foreach(Transform t in pool.transform)
        {
            if(t.name.Contains("Mine"))
            {
                t.gameObject.SetActive(state);
            }
        }
    }

    private void HandleControllerInputs()
    {
        if (Input.GetButtonDown("Xbox_Validation") && OnPause && ButtonSelected == 1)
        {
            SetPauseOff();
            StartCoroutine(BlockInput(.15f));
        }
        if (Input.GetButtonDown("Xbox_Validation") && OnPause && ButtonSelected == 2)
        {
            SetOptionsOn();
            StartCoroutine(BlockInput(.15f));
        }
        if (Input.GetButtonDown("Xbox_Validation") && OnPause && ButtonSelected == 3)
        {
            QuitFirstStep();
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

    public int confirmationPosition = 1;
    private void HandleControllerInputsConfirmation()
    {
        
        if (Input.GetAxisRaw("Xbox_Horizontal") == 1 && !inputBlocked && confirmationPosition + 1 < 3)
        {
            StartCoroutine(BlockInput(.15f));
            confirmationPosition++;
        }
        if (Input.GetAxisRaw("Xbox_Horizontal") == -1 && !inputBlocked && confirmationPosition - 1 > 0)
        {
            StartCoroutine(BlockInput(.15f));
            confirmationPosition--;
        }

        for (int i = 3; i < 5; i++)
        {
            buttons[i].transform.localScale = new Vector2(1, 1);
        }

        buttons[2 + confirmationPosition].transform.localScale = new Vector2(1.35f, 1.35f);


        if (Input.GetButtonDown("Xbox_Validation") && confirmationPosition == 1)
        {
            //Quitter
            GameObject player = GameObject.Find("Player");
            StartCoroutine(player.GetComponent<PlayerManager>().ILoadMenu());
            SetPauseOff();
            player.transform.position = new Vector2(100, 100);
            Time.timeScale = 1;
            StartCoroutine(BlockInput(4f));
        }
        if (Input.GetButtonDown("Xbox_Validation") && confirmationPosition == 2 || Input.GetButtonDown("Xbox_B"))
        {
            //Retour en arrière
            ReverseFirstStep();
            StartCoroutine(BlockInput(.15f));
        }
    }

    private int volume = 5;

    private void HandleControllerInputsOptions()
    {
        if (Input.GetButtonDown("Xbox_B"))
        {
            SetOptionsOff();
            StartCoroutine(BlockInput(.15f));
        }
        //Volume = 0 à 10
        if (Input.GetAxisRaw("Xbox_Horizontal") == 1 && !inputBlocked && volume + 1 < 11)
        {
            StartCoroutine(BlockInput(.15f));
            volume++;
            UpdateSound();
        }
        if (Input.GetAxisRaw("Xbox_Horizontal") == -1 && !inputBlocked && volume - 1 > -1)
        {
            StartCoroutine(BlockInput(.15f));
            volume--;
            UpdateSound();
        }

        
        
    }

    private void UpdateSound()
    {
        //Bouge la boule sur la barre de volume
        switch (volume)
        {
            case 0: boule.localPosition = new Vector2(-115, boule.localPosition.y); break;
            case 1: boule.localPosition = new Vector2(-92, boule.localPosition.y); break;
            case 2: boule.localPosition = new Vector2(-69, boule.localPosition.y); break;
            case 3: boule.localPosition = new Vector2(-46, boule.localPosition.y); break;
            case 4: boule.localPosition = new Vector2(-23, boule.localPosition.y); break;
            case 5: boule.localPosition = new Vector2(0, boule.localPosition.y); break;
            case 6: boule.localPosition = new Vector2(23, boule.localPosition.y); break;
            case 7: boule.localPosition = new Vector2(46, boule.localPosition.y); break;
            case 8: boule.localPosition = new Vector2(69, boule.localPosition.y); break;
            case 9: boule.localPosition = new Vector2(92, boule.localPosition.y); break;
            case 10: boule.localPosition = new Vector2(115, boule.localPosition.y); break;
        }

        //Update le texte à l'écran
        volumeText.text = $"{volume}";

        for(int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = (float)volume/10;
        }
            
        //Update son + feedback
    }

}
