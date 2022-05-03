using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Upgrade
{
    [Header("Settings")]
    public float timeBetweenDashs = 1f;
    public float dashForceKeyboard = 5f;
    public float dashForceController = 30f;
    public float dashTime = 0.05f;

    //If the player can dash
    private bool canDash = true;
    private float cpt = 0;

    //player movement script and rigidbody
    private Player_Movement pm;
    private Rigidbody2D rb;

    //dashh sound
    private SoundManager soundManager;
    private TempTrail Temp;
    private GameObject DashTrail;

    private void Awake()
    {
        pm = GetComponentInParent<Player_Movement>();
        rb = GetComponentInParent<Rigidbody2D>();
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
        Temp = GameObject.Find("TempTrail").GetComponent<TempTrail>();
    }

    private void Start()
    {
        Temp.ActiveTrail();
        DashTrail = GameObject.Find("DashTrail");
        DashTrail.SetActive(false);
    }


    private void Update()
    {

        //Timer to allow dash after using it
        if(!canDash)
        {
            cpt += Time.deltaTime;
            if(cpt >= timeBetweenDashs)
            {
                canDash = true;
                cpt = 0;
            }
        }

        if(pm.UseKeyboardSettings && canDash)
        {
            //If the input is pressed and the player can dash
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Gets the mouse position and the direction from the player
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mousePos - (Vector2)pm.transform.position;

                //Normalize the vector
                direction.Normalize();

                //Disallow movement
                pm.canMove = false;

                //Applies the dash force
                rb.velocity = direction * dashForceKeyboard;

                //Removes the ability to dash
                canDash = false;

                //Timer to allow movement again
                StartCoroutine(DashTempo(dashTime));
            }
        }

        if(pm.UseControllerSettings && canDash)
        {
            if(Input.GetAxis("Xbox_RB") > 0.9f)
            {
                pm.movement.Normalize();

                soundManager.playAudioClip(11);

                //Disallow movement
                pm.canMove = false;

                //Applies the dash force
                rb.velocity = pm.movement * dashForceController;
                
                //Removes the ability to dash
                canDash = false;

                //Timer to allow movement again
                StartCoroutine(DashTempo(dashTime));

                DashTrail.SetActive(true);
            }
        }
        
    }

    IEnumerator DashTempo(float t)
    {
        yield return new WaitForSeconds(t);
        rb.velocity = Vector2.zero;
        pm.canMove = true;
        DashTrail.SetActive(false);
    }



}
