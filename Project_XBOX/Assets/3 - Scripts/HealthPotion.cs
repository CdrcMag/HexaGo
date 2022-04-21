using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPotion : Collectable
{
    private GameObject HealthPotionParticles;

    private void Awake()
    {
        HealthPotionParticles = Resources.Load<GameObject>("Health Potion Particles");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.AddHealthPoint(PlayerManager.Instance.HealAmount);

            GameObject particles_instance = Instantiate(HealthPotionParticles, transform.position, Quaternion.identity);
            Destroy(particles_instance, 1f);

            Destroy(gameObject);
        }
            
    }


}
