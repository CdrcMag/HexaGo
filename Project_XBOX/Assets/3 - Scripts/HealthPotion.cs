using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPotion : Collectable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.AddHealthPoint(PlayerManager.Instance.HealAmount);
            Destroy(gameObject);
        }
            
    }


}