using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponSelectorRemake : MonoBehaviour
{
    public static WeaponSelectorRemake Instance;

    public GameObject WeaponSelectionMenu;
    public GameObject SlotSelectionMenu;

    //Player's shooting script
    private Player_Shooting PlayerShooting;

    //Icones des armes et upgrades
    public Sprite[] icons;
    //Textes
    public TextMeshProUGUI txtWeapon01;
    public TextMeshProUGUI txtWeapon02;
    //Icones
    public Image iconWeapon01;
    public Image iconWeapon02;
    //Selector
    public Image selector;
    //Weapon selected
    public int selectedID = -1;

    //1. Afficher premier menu
    //2. Générer armes et upgrades
    //3. Afficher ces armes et upgrades sur les boutons
    //2. Selection du joueur
    //3. Afficher 2ème menu
    //4. Selectionner emplacement
    //5. Assigner arme ou upgrade à emplacement


    private void Awake()
    {
        //Singleton de ce script
        if (Instance == null) Instance = this;

        //Get player shooting system
        PlayerShooting = GameObject.Find("Player").GetComponent<Player_Shooting>();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Initialisation();
        }

        if(WeaponSelectionMenu.activeInHierarchy == true)
        {
            //120 et -120
            if(Input.GetButtonDown("Xbox_Horizontal"))
            {

            }
        }


    }

    public void Initialisation()
    {
        Pause_System.Instance.SetPauseOn();

        UpgradesAndWeapons firstWeapon = GenerateUpgradeAndWeapon();
        UpgradesAndWeapons secondWeapon = GenerateUpgradeAndWeapon();
        for (int i = 0; i < 10; i++)
        {
            if (secondWeapon == firstWeapon) secondWeapon = GenerateUpgradeAndWeapon();
            else i = 10;
        }

        txtWeapon01.text = firstWeapon.name;
        txtWeapon02.text = secondWeapon.name;

        SetIconOnImage(firstWeapon, iconWeapon01);
        SetIconOnImage(secondWeapon, iconWeapon02);

        SetMenusStates(true, false);

        selector.rectTransform.localPosition = new Vector2(-120, 0);
        selectedID = 0;
    }


    //Génère une arme ou une upgrade
    private UpgradesAndWeapons GenerateUpgradeAndWeapon()
    {
        //Créer une liste avec toutes les armes et toutes les upgrades
        List<UpgradesAndWeapons> newUpgradeAndWeapons = new List<UpgradesAndWeapons>();
        foreach(Weapon w in PlayerShooting.ArmesFront)
        {
            newUpgradeAndWeapons.Add(w);
        }
        foreach (Upgrade u in PlayerShooting.Upgrades)
        {
            newUpgradeAndWeapons.Add(u);
        }

        UpgradesAndWeapons a = newUpgradeAndWeapons[Random.Range(0, newUpgradeAndWeapons.Count)];

        return a;

    }

    private void SetIconOnImage(UpgradesAndWeapons i, Image img)
    {

        if (i.name == "Canon") img.sprite = icons[0]; 
        else if (i.name == "BigFuckingGun") img.sprite = icons[1]; 
        else if (i.name == "Mitraillette") img.sprite = icons[2]; 
        else if (i.name == "FusilPompe") img.sprite = icons[3]; 
        else if (i.name == "Propulseur") img.sprite = icons[4]; 
        else if (i.name == "Shield") img.sprite = icons[5]; 
        else if (i.name == "Dash") img.sprite = icons[6]; 
        else if (i.name == "Totems") img.sprite = icons[7]; 


    }


    //Set les états des menus
    public void SetMenusStates(bool weaponSelectionMenuState, bool SlotSelectionMenuState)
    {
        WeaponSelectionMenu.SetActive(weaponSelectionMenuState);
        SlotSelectionMenu.SetActive(SlotSelectionMenuState);
    }

}
