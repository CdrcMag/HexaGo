using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private float lifePoint = 100;
    public float reduction = 0;//Percentage ( 0 - 1 )

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _damage) 
    {
        float damageToTake = _damage - (_damage * reduction);

        lifePoint -= damageToTake;
        
        if(lifePoint <= 0)
        {
            Die();
        }
    }

    public float GetLifePoint() { return lifePoint; }

    // =====================================================


    private void Die()
    {
        Destroy(gameObject);
    }
}
