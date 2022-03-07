using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : Upgrade
{
    public GameObject totemPrefab;

    const int TOTEM_MAX = 3;

    public int totalTotemPlaced = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && totalTotemPlaced < TOTEM_MAX)
        {
            GameObject totemInstance = Instantiate(totemPrefab, transform.position, Quaternion.identity);
            totalTotemPlaced += 1;
            totemInstance.GetComponent<TotemInstance>().totemMain = this;
        }
    }



}
