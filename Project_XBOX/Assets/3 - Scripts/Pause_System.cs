using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_System : MonoBehaviour
{
    public static Pause_System Instance = null;

    private GameObject PauseMenu;

    private bool OnPause = false;//Not on pause

    private void Awake()
    {
        if(Instance != null)
            Instance = this;

        PauseMenu = GameObject.Find("Pause").GetComponent<RectTransform>().GetChild(0).gameObject;

        SetPauseOff();

        StartCoroutine(LikeAnUpdate());
    }

    IEnumerator LikeAnUpdate()
    {
        while(true)
        {
            if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape))
            {
                if (OnPause) SetPauseOff();
                else if (!OnPause) SetPauseOn();
            }

            yield return null;
        }
    }
    

    public void SetPauseOn()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        OnPause = true;

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
}
