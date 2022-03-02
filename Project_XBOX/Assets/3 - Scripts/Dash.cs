using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Upgrade
{

    public float timeBetweenDashs = 1f;
    public float dashForce = 5f;
    public float dashTime = 0.1f;

    private bool canDash = true;
    private float cpt = 0;

    private Player_Movement pm;
    private Rigidbody2D rb;

    private void Awake()
    {
        pm = GetComponentInParent<Player_Movement>();
        rb = GetComponentInParent<Rigidbody2D>();
    }


    private void Update()
    {
        if(!canDash)
        {
            cpt += Time.deltaTime;
            if(cpt >= timeBetweenDashs)
            {
                canDash = true;
                cpt = 0;
            }
        }


        if(Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - (Vector2)pm.transform.position;
            direction.Normalize();
            pm.canMove = false;
            rb.velocity = direction * dashForce;
            canDash = false;
            StartCoroutine(DashTempo(dashTime));
        }
    }

    IEnumerator DashTempo(float t)
    {
        yield return new WaitForSeconds(t);
        pm.canMove = true;
    }



}
