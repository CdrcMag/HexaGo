using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Upgrade
{

    public float reduction = 0.3f;//réduction de 30%
    public GameObject shieldEffect; // Effet visuel du shiel

    private void Start()
    {
        if(shieldEffect != null)
        {
            shieldEffect.SetActive(true);
        }
        else
        {
            print("L'effet visuel du bouclier n'est pas attribué dans l'inspector.");
        }

        GameObject.Find("Player").GetComponent<PlayerManager>().reduction += reduction;
    }
}
