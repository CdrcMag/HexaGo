using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recap : MonoBehaviour
{
    public Transform SlotFront;
    public Transform SlotFrontLeft;
    public Transform SlotFrontRight;
    public Transform SlotBack;
    public Transform SlotBackLeft;
    public Transform SlotBackRight;

    public GameObject img;

    private void Start()
    {
        Instantiate(img.gameObject, SlotFront.localPosition, Quaternion.identity);
    }





}
