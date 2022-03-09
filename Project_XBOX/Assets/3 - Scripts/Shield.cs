using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Upgrade
{

    public float reduction = 0.3f;//réduction de 30%
    private void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerManager>().reduction += reduction;
    }
}
