using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    //Menu
    public GameObject weaponSelectionMenu;
    private GameObject firstMenu;
    private GameObject mainMenu;

    //Player shooting script
    private Player_Shooting ps;

    //Temp weapons generated here
    private Weapon tempWeapon_1;
    private Weapon tempWeapon_2;

    //Temp upgrades generated here
    private Upgrade tempUpgrade_1;
    private Upgrade tempUpgrade_2;

    int pattern = -1;

    private void Awake()
    {
        ps = GameObject.Find("Player").GetComponent<Player_Shooting>();

        weaponSelectionMenu.SetActive(true);

       
        
    }

    private void Start()
    {
        //firstMenu = weaponSelectionMenu.transform.GetChild(0).gameObject;
        firstMenu = weaponSelectionMenu.transform.GetChild(0).gameObject;
        mainMenu = weaponSelectionMenu.transform.GetChild(1).gameObject;

        firstMenu.SetActive(false);
        mainMenu.SetActive(false);

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && firstMenu.activeSelf == false)
        {

            SetStates(true, false);

            GenerateTwoAmongEverything();
            
            

        }
    }

    private void SetStates(bool a, bool b)
    {
        if (a) 
            Time.timeScale = 0;
        else 
            Time.timeScale = 1;

        firstMenu.SetActive(a);
        mainMenu.SetActive(b);

       
    }


    //Appliquer l'arme à emplacement choisi
    public void SetWeaponOnSlot(string slot)
    {
        //Récupère l'enum depuis un string passé en argument dans chaque bouton
        Player_Shooting.SlotName enumState = (Player_Shooting.SlotName)System.Enum.Parse(typeof(Player_Shooting.SlotName), slot);

        if (pattern == 0 && selectedWeapon == 0) ps.SetWeapon(enumState, tempWeapon_1);
        if (pattern == 0 && selectedWeapon == 1) ps.SetWeapon(enumState, tempWeapon_2);

        if (pattern == 1 && selectedWeapon == 0) ps.SetUpgrade(enumState, tempUpgrade_1);
        if (pattern == 1 && selectedWeapon == 1) ps.SetUpgrade(enumState, tempUpgrade_2);

        if (pattern == 2 && selectedWeapon == 0) ps.SetWeapon(enumState, tempWeapon_1);
        if (pattern == 2 && selectedWeapon == 1) ps.SetUpgrade(enumState, tempUpgrade_1);

        SetStates(false, false);

    }

    private int selectedWeapon = -1;

    public void SetWeaponToButton(int id)
    {
        selectedWeapon = id;

        firstMenu.SetActive(false);
        mainMenu.SetActive(true);
    }




    private void GenerateTwoRandomWeapons() => StartCoroutine(IGenerateWeapons());
    private void GenerateTwoRandomUpgrades() => StartCoroutine(IGenerateUpgrades());

    private void GenerateTwoAmongEverything()
    {

        //0 = 2 armes
        //1 = 2 upgrades
        //2 = 1 de chaque

        pattern = Random.Range(0, 3);

        if (pattern == 0) { GenerateTwoRandomWeapons(); }
        if (pattern == 1) { GenerateTwoRandomUpgrades(); }
        if (pattern == 2)
        {
            tempWeapon_1 = GetRandomWeapon();
            tempUpgrade_1 = GetRandomUpgrade();

            print($"1 : {tempWeapon_1} - 2 : {tempUpgrade_1}");
        }
    }
    
    IEnumerator IGenerateWeapons()
    {
        tempWeapon_1 = GetRandomWeapon();
        tempWeapon_2 = GetRandomWeapon();

        while(tempWeapon_1 == tempWeapon_2)
        {
            tempWeapon_2 = GetRandomWeapon();
            yield return null;
        }

        print($"1 : {tempWeapon_1} - 2 : {tempWeapon_2}");
        
    }
    IEnumerator IGenerateUpgrades()
    {
        tempUpgrade_1 = GetRandomUpgrade();
        tempUpgrade_2 = GetRandomUpgrade();

        while (tempUpgrade_1 == tempUpgrade_2)
        {
            tempUpgrade_2 = GetRandomUpgrade();
            yield return null;
        }

        print($"1 : {tempUpgrade_1} - 2 : {tempUpgrade_2}");
    }

    Weapon GetRandomWeapon()
    {
        int rand = Random.Range(0, ps.ArmesFront.Length);

        return ps.ArmesFront[rand];
    }
    Upgrade GetRandomUpgrade()
    {
        int rand = Random.Range(0, ps.Upgrades.Length);

        return ps.Upgrades[rand];
    }
}
