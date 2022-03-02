using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Upgrade
{
    [Header("Settings")]
    public float timeBetweenDashs = 1f;
    public float dashForce = 5f;
    public float dashTime = 0.1f;

    //If the player can dash
    private bool canDash = true;
    private float cpt = 0;

    //player movement script and rigidbody
    private Player_Movement pm;
    private Rigidbody2D rb;

    private void Awake()
    {
        pm = GetComponentInParent<Player_Movement>();
        rb = GetComponentInParent<Rigidbody2D>();
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

        
        //If the input is pressed and the player can dash
        if(Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            //Gets the mouse position and the direction from the player
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - (Vector2)pm.transform.position;

            //Normalize the vector
            direction.Normalize();

            //Disallow movement
            pm.canMove = false;

            //Applies the dash force
            rb.velocity = direction * dashForce;

            //Removes the ability to dash
            canDash = false;

            //Timer to allow movement again
            StartCoroutine(DashTempo(dashTime));
        }
    }

    IEnumerator DashTempo(float t)
    {
        yield return new WaitForSeconds(t);
        pm.canMove = true;
    }



}
