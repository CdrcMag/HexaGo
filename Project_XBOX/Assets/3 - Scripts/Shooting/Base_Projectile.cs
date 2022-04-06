using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Projectile : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField]
    protected float newScale = 1;

    [Header("Bounce")]
    [SerializeField]
    protected bool canBounce = false;

    [SerializeField]
    protected int nbrOfBounceMax;
    private int nbrOfBounce = 0;
    public int id;

    private void Start()
    {
        if(canBounce)
        {
            PhysicsMaterial2D pm = new PhysicsMaterial2D();
            pm.bounciness = 1;
            pm.friction = 0;

            GetComponent<Rigidbody2D>().sharedMaterial = pm;
        }

        if (newScale != 1)
            SetNewScale(newScale);
        
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Block")) && canBounce)
        {
            BounceCheck();
        }
    }


    public virtual void SetNewScale(float nScale)
    {
        transform.localScale = transform.localScale * nScale;
    }


    public void BounceCheck()
    {
        if (nbrOfBounce == nbrOfBounceMax)
            Destroy(gameObject);
        else
            nbrOfBounce += 1;
    }



}
