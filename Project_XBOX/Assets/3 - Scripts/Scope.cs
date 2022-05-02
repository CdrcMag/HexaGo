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
        if(Input.GetAxisRaw("Controller_LeftTrigger") == 1 && spriteRenderer.isVisible == false)
        {
            spriteRenderer.enabled ^= true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
