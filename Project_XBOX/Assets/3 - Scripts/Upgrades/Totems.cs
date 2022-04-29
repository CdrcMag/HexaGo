using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : Upgrade
{
    public GameObject totemPrefab;

    public Sprite imgTotem;

    private Transform player;

    const int TOTEM_MAX = 3;

    public int totalTotemPlaced = 0;

    private float cpt = 1f;
    private float timer = 0;
    private bool canPlaceTotem = true;

    private void Awake()
    {
        player = transform.root;
    }

    private void Update()
    {
        if(!canPlaceTotem)
        {
            timer += Time.deltaTime;
            if(timer >= cpt)
            {
                timer = 0;
                canPlaceTotem = true;
            }
        }


        if(Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Controller_LeftTrigger") > 0.9f)
        {
            if(totalTotemPlaced < TOTEM_MAX && canPlaceTotem)
            {
                GameObject totemInstance = Instantiate(totemPrefab, player.position, Quaternion.identity);
                totemInstance.GetComponent<SpriteRenderer>().sprite = imgTotem;
                totalTotemPlaced += 1;
                totemInstance.GetComponent<TotemInstance>().totemMain = this;
                canPlaceTotem = false;
            }
            
        }
    }



}
