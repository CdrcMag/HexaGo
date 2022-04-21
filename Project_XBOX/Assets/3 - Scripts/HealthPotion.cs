using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPotion : Collectable
{
    public int healAmount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.AddHealthPoint(healAmount);
            Destroy(gameObject);
        }
            
    }


}
