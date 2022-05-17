using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Recap : MonoBehaviour
{

    [Header("Slots")]
    public RectTransform SlotFront;
    public RectTransform SlotFrontLeft;
    public RectTransform SlotFrontRight;
    public RectTransform SlotBack;
    public RectTransform SlotBackLeft;
    public RectTransform SlotBackRight;

    [Header("Icones")]
    public GameObject img_BFG;
    public GameObject img_Canon;
    public GameObject img_Dash;
    public GameObject img_MachineGun;
    public GameObject img_Propulsor;
    public GameObject img_Shield;
    public GameObject img_Shotgun;
    public GameObject img_Totem;
    public GameObject img_Drone;
    public GameObject img_Croissant;
    public GameObject img_Tortuga;

    [Header("Textes recap")]
    public TextMeshProUGUI TotalEnnemis;
    public TextMeshProUGUI TotalSalles;
    public TextMeshProUGUI HealRamasses;
    public TextMeshProUGUI TotalTemps;
    public TextMeshProUGUI Morts;

    //Player's shooting system
    private Player_Shooting ps;
    private Transform enemyPool;

    private void OnEnable()
    {
        enemyPool = GameObject.Find("EnemyPool").transform;
    }

    private void Start()
    {
        ps = GameObject.Find("Player").GetComponent<Player_Shooting>();
        
    }

    public void GenerateIcons()
    {
        UpdateStats();
        SetMineLightsStates(false);


        List<RectTransform> slots = new List<RectTransform>();
        slots.Add(SlotFront);
        slots.Add(SlotFrontLeft);
        slots.Add(SlotFrontRight);
        slots.Add(SlotBack);
        slots.Add(SlotBackLeft);
        slots.Add(SlotBackRight);

        List<Player_Shooting.SlotName> slotNames = new List<Player_Shooting.SlotName>();
        slotNames.Add(Player_Shooting.SlotName.Front);
        slotNames.Add(Player_Shooting.SlotName.FrontLeft);
        slotNames.Add(Player_Shooting.SlotName.FrontRight);
        slotNames.Add(Player_Shooting.SlotName.Back);
        slotNames.Add(Player_Shooting.SlotName.BackLeft);
        slotNames.Add(Player_Shooting.SlotName.BackRight);

        int place = 0;

        foreach (Player_Shooting.SlotName r in slotNames)
        {
            string i = ps.GetWeaponOnSlot(r);

            if (i != "")
            {
                if (i == "Canon") Spawn(img_Canon, slots[place].position, slots[place]);
                if (i == "Mitraillette") Spawn(img_MachineGun, slots[place].position, slots[place]);
                if (i == "Shotgun") Spawn(img_Shotgun, slots[place].position, slots[place]);
                if (i == "BFG") Spawn(img_BFG, slots[place].position, slots[place]);
                if (i == "Shield") Spawn(img_Shield, slots[place].position, slots[place]);
                if (i == "Totems") Spawn(img_Totem, slots[place].position, slots[place]);
                if (i == "Propulseur") Spawn(img_Propulsor, slots[place].position, slots[place]);
                if (i == "Dash") Spawn(img_Dash, slots[place].position, slots[place]);
                if (i == "Drone") Spawn(img_Drone, slots[place].position, slots[place]);
                if (i == "Croissant") Spawn(img_Croissant, slots[place].position, slots[place]);
                if (i == "Tortuga") Spawn(img_Tortuga, slots[place].position, slots[place]);
            }

            place++;
        }

    }

    private void Spawn(GameObject img, Vector3 pos, RectTransform r)
    {
        img.GetComponent<SpriteRenderer>().sortingOrder = 25;
        img.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);

        Instantiate(img, pos, Quaternion.identity, r);
    }


    private void OnDisable()
    {
        if (SlotFront.childCount > 0) Destroy(SlotFront.GetChild(0).gameObject);
        if (SlotFrontLeft.childCount > 0) Destroy(SlotFrontLeft.GetChild(0).gameObject);
        if (SlotFrontRight.childCount > 0) Destroy(SlotFrontRight.GetChild(0).gameObject);
        if (SlotBack.childCount > 0) Destroy(SlotBack.GetChild(0).gameObject);
        if (SlotBackLeft.childCount > 0) Destroy(SlotBackLeft.GetChild(0).gameObject);
        if (SlotBackRight.childCount > 0) Destroy(SlotBackRight.GetChild(0).gameObject);

        SetMineLightsStates(true);
    }

    private void UpdateStats()
    {
        TotalEnnemis.text = PlayerPrefs.GetInt("Nbr_EnnemisTues").ToString();
        TotalSalles.text = PlayerPrefs.GetInt("Nbr_TotalSalles").ToString();
        HealRamasses.text = PlayerPrefs.GetInt("Nbr_HealRamasses").ToString();
        Morts.text = PlayerPrefs.GetInt("Nbr_Morts").ToString();

        //Temps
        float t = PlayerPrefs.GetFloat("TimePlayed");
        t = (int)t;
        TimeSpan a = TimeSpan.FromSeconds(t);
        
        if(a.Days>=1)
            TotalTemps.text = $"{a.Days}d{a.Hours}h{a.Minutes}min{a.Seconds}s";
        else
            TotalTemps.text = $"{a.Hours}h{a.Minutes}min{a.Seconds}s";


    }

    public void SetMineLightsStates(bool state)
    {
        if(enemyPool != null)
        {
            foreach (Transform t in enemyPool)
            {
                if (t.name.Contains("Mine"))
                {
                    t.GetChild(1).gameObject.SetActive(state);
                }
            }
        }
        
    }

    

}
