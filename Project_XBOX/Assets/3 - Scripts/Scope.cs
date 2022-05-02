using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Controller_LeftTrigger") > 0.6f)
        {
            spriteRenderer.enabled ^= true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
