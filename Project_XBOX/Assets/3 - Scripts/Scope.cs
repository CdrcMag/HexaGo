using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private GameObject childOne;

    void Start()
    {
        childOne = transform.GetChild(0).gameObject;
        childOne.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Controller_LeftTrigger") == 1)
        {
            childOne.SetActive(true);
        }
        else
        {
            childOne.SetActive(false);
        }
    }
}
