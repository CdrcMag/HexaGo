using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Vector3 STARTPOS = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion STARTROT = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // ===================== VARIABLES =====================

    [Header("Drag from Scene")]
    public RoomManager roomManager;
    public Transform player;
    public Transform background;
    public AudioSource musicManager;
    public Transform tutorialRoot;

    [Header("Drag from Assets")]
    public AudioClip tutoTheme;

    private int state = 0;

    // =====================================================

    public void LoadTutorial()
    {
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Player_Movement>().canMove = true;

        for(int i = 0; i < background.childCount; i++)
        {
            background.GetChild(i).gameObject.SetActive(false);
        }

        musicManager.Stop();
        musicManager.clip = tutoTheme;
        musicManager.Play();

        tutorialRoot.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(state == 0)
        {
            CheckState00();
        }
        else if(state == 1)
        {
            CheckState01();
        }
    }

    private void CheckState00()
    {
        if(player.position != STARTPOS && player.rotation != STARTROT)
        {
            tutorialRoot.GetChild(1).gameObject.SetActive(false);
            tutorialRoot.GetChild(2).gameObject.SetActive(true);

            state++;
        }
    }

    private void CheckState01()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f)
        {
            tutorialRoot.GetChild(2).gameObject.SetActive(false);

            state++;
        }
    }
}
