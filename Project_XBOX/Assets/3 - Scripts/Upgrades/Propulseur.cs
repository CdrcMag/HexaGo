using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulseur : Upgrade
{
    [Range(0,1)]
    public float movementSpeedBoost;

    private void Start()
    {
        GetComponentInParent<Player_Movement>().speedBoost += movementSpeedBoost;
    }



}
