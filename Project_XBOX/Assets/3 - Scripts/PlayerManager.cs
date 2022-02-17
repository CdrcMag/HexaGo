using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private float lifePoint = 100;

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _lifePoint) 
    { 
        lifePoint = _lifePoint;
        
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
