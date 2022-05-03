using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTrail : MonoBehaviour
{
    private GameObject DashTrail;
    // Start is called before the first frame update
    void Start()
    {
        DashTrail = GameObject.Find("DashTrail");
        DashTrail.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveTrail()
    {
        DashTrail.SetActive(true);
    }
}
