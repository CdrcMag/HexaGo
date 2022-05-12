using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_System : MonoBehaviour
{
    public static Pause_System Instance = null;

    public GameObject PauseMenu;

    private bool OnPause = false;//Not on pause
    [HideInInspector] public bool canPause = true;

    public float TimePlayed = 0;

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
        a = PauseMenu.transform.GetChild(1).gameObject;
        a.GetComponent<Recap>().GenerateIcons();

    }

    public GameObject a;

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
}
