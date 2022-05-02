using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponSelectorRemake : MonoBehaviour
{
    public static WeaponSelectorRemake Instance;

    //Menus
    public GameObject WeaponSelectionMenu;
    public GameObject SlotSelectionMenu;
    //Player's shooting script
    private Player_Shooting PlayerShooting;
    //Icones des armes et upgrades
    public Sprite[] icons;
    //Images des slots
    public Image[] iconWeaponSlot;
    //Textes
    public TextMeshProUGUI txtWeapon01;
    public TextMeshProUGUI txtWeapon02;
    //Icones
    public Image iconWeapon01;
    public Image iconWeapon02;
    //Selector
    public Image selector;
    public Image selector2;
    //Weapon selected
    public int selectedID = -1;
    //Slot selected
    public int selectedSlotX = -1;
    public int selectedSlotY = -1;
    //If the inputs are blocked
    private bool inputBlocked = false;
    //Current selected weapon or upgrade
    private UpgradesAndWeapons temporaryFirstWeaponOrUpgrade;
    private UpgradesAndWeapons temporarySecondWeaponOrUpgrade;
    private UpgradesAndWeapons finalWeaponOrUpgrade;
    //Slots parent
    public Transform slots;

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

        //print("H:" + Input.GetAxis("Xbox_Horizontal") + "///V: " + Input.GetAxis("Xbox_Vertical"));

        //Selection de l'arme ou upgrade dans le premier menu
        if(WeaponSelectionMenu.activeInHierarchy == true && !inputBlocked)
        {
            if(Input.GetAxisRaw("Xbox_Horizontal") == 1)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedID = 1;
                selector.rectTransform.localPosition = new Vector2(120, 0);
            }
            if (Input.GetAxisRaw("Xbox_Horizontal") == -1)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedID = 0;
                selector.rectTransform.localPosition = new Vector2(-120, 0);
            }

            if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Xbox_Validation"))
            {
                if (selectedID == 0) finalWeaponOrUpgrade = temporaryFirstWeaponOrUpgrade;
                if (selectedID == 1) finalWeaponOrUpgrade = temporarySecondWeaponOrUpgrade;

                SetMenusStates(false, true);

                //Etat des armes et upgrades sur les boutons
                ShowWeaponOnSlot();

                selectedSlotX = 2;
                selectedSlotY = 2;

                StartCoroutine(BlockInput(0.15f));
            }
        }

        if(SlotSelectionMenu.activeInHierarchy == true && !inputBlocked)
        {
            if (Input.GetAxisRaw("Xbox_Horizontal") == 1 && selectedSlotX + 1 <= 3)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedSlotX += 1;
            }
            if (Input.GetAxisRaw("Xbox_Horizontal") == -1 && selectedSlotX - 1 >= 1)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedSlotX -= 1;
            }
            if (Input.GetAxisRaw("Xbox_Vertical") == 1 && selectedSlotY + 1 <= 2)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedSlotY += 1;
            }
            if (Input.GetAxisRaw("Xbox_Vertical") == -1 && selectedSlotY - 1 >= 1)
            {
                StartCoroutine(BlockInput(0.15f));
                selectedSlotY -= 1;
            }

            if (selectedSlotX == 1 && selectedSlotY == 1) selector2.rectTransform.localPosition = new Vector2(-90, -60);
            if (selectedSlotX == 2 && selectedSlotY == 1) selector2.rectTransform.localPosition = new Vector2(0, -60);
            if (selectedSlotX == 3 && selectedSlotY == 1) selector2.rectTransform.localPosition = new Vector2(90, -60);
            if (selectedSlotX == 1 && selectedSlotY == 2) selector2.rectTransform.localPosition = new Vector2(-90, 30);
            if (selectedSlotX == 2 && selectedSlotY == 2) selector2.rectTransform.localPosition = new Vector2(0, 30);
            if (selectedSlotX == 3 && selectedSlotY == 2) selector2.rectTransform.localPosition = new Vector2(90, 30);

            if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Xbox_Validation"))
            {
                string slotName = "";
                if (selectedSlotX == 1 && selectedSlotY == 1) slotName = "BackLeft";
                else if (selectedSlotX == 2 && selectedSlotY == 1) slotName = "Back";
                else if (selectedSlotX == 3 && selectedSlotY == 1) slotName = "BackRight";
                else if (selectedSlotX == 1 && selectedSlotY == 2) slotName = "FrontLeft";
                else if (selectedSlotX == 2 && selectedSlotY == 2) slotName = "Front";
                else if (selectedSlotX == 3 && selectedSlotY == 2) slotName = "FrontRight";

                if (HowMuchGuns() == 1 && isAWeaponOnTheSelectedSlot() == true)
                    return;

                if(slotName != "" )
                {
                    //Récupère l'enum depuis un string passé en argument dans chaque bouton
                    Player_Shooting.SlotName enumState = (Player_Shooting.SlotName)System.Enum.Parse(typeof(Player_Shooting.SlotName), slotName);
                    
                    //Upgrade ou weapon
                    if (finalWeaponOrUpgrade.isWeapon == true) PlayerShooting.SetWeapon(enumState, (Weapon)finalWeaponOrUpgrade);
                    if (finalWeaponOrUpgrade.isWeapon == false) PlayerShooting.SetUpgrade(enumState, (Upgrade)finalWeaponOrUpgrade);

                    StartCoroutine(BlockInput(0.1f));

                    selectedID = -1;
                    selectedSlotX = 2;
                    selectedSlotY = 2;

                    SetMenusStates(false, false);
                    Pause_System.Instance.canPause = true;
                    Time.timeScale = 1;

                }
            }
        }


    }

    IEnumerator BlockInput(float s)
    {
        inputBlocked = true;
        yield return new WaitForSecondsRealtime(s);
        inputBlocked = false;
    }

    public void Initialisation()
    {
        Pause_System.Instance.canPause = false;
        Time.timeScale = 0;

        UpgradesAndWeapons firstWeapon = GenerateUpgradeAndWeapon();
        UpgradesAndWeapons secondWeapon = GenerateUpgradeAndWeapon();
        for (int i = 0; i < 10; i++)
        {
            if (secondWeapon == firstWeapon) secondWeapon = GenerateUpgradeAndWeapon();
            else i = 10;
        }

        temporaryFirstWeaponOrUpgrade = firstWeapon;
        temporarySecondWeaponOrUpgrade = secondWeapon;

        txtWeapon01.text = firstWeapon.name;
        txtWeapon02.text = secondWeapon.name;

        SetIconOnImage(firstWeapon, iconWeapon01);
        SetIconOnImage(secondWeapon, iconWeapon02);

        SetMenusStates(true, false);

        selector.rectTransform.localPosition = new Vector2(-120, 0);
        selectedID = 0;
    }

    private void ShowWeaponOnSlot()
    {
        for (int i = 0; i < slots.childCount; i++)
        {
            if (slots.GetChild(i).childCount == 1)
            {
                if (slots.GetChild(i).GetChild(0).name == "Canon(Clone)") iconWeaponSlot[i].sprite = icons[0];
                else if (slots.GetChild(i).GetChild(0).name == "BigFuckingGun(Clone)") iconWeaponSlot[i].sprite = icons[1];
                else if (slots.GetChild(i).GetChild(0).name == "Mitraillette(Clone)") iconWeaponSlot[i].sprite = icons[2];
                else if (slots.GetChild(i).GetChild(0).name == "FusilPompe(Clone)") iconWeaponSlot[i].sprite = icons[3];
                else if (slots.GetChild(i).GetChild(0).name == "Propulseur(Clone)") iconWeaponSlot[i].sprite = icons[4];
                else if (slots.GetChild(i).GetChild(0).name == "Shield(Clone)") iconWeaponSlot[i].sprite = icons[5];
                else if (slots.GetChild(i).GetChild(0).name == "Dash(Clone)") iconWeaponSlot[i].sprite = icons[6];
                else if (slots.GetChild(i).GetChild(0).name == "Totems(Clone)") iconWeaponSlot[i].sprite = icons[7];
            }
        }
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

        if(a.GetType() == typeof(Canon) || a.GetType() == typeof(Mitraillette) || a.GetType() == typeof(BigFuckingGun) || a.GetType() == typeof(FusilPompe))
        {
            a.isWeapon = true;
        }

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

    int HowMuchGuns()
    {
        int total = 0;

        foreach (KeyValuePair<Player_Shooting.SlotName, Player_Shooting.WeaponName> pair in PlayerShooting.currentWeaponsState)
        {
            if (pair.Value == Player_Shooting.WeaponName.BigFuckingGun) total++;
            if (pair.Value == Player_Shooting.WeaponName.Canon) total++;
            if (pair.Value == Player_Shooting.WeaponName.Mitraillette) total++;
            if (pair.Value == Player_Shooting.WeaponName.Shotgun) total++;
        }

        return total;

    }

    private bool isAWeaponOnTheSelectedSlot()
    {
        Player_Shooting.SlotName sTemp = Player_Shooting.SlotName.None;

        if (selectedSlotX == 1 && selectedSlotY == 1) sTemp = Player_Shooting.SlotName.BackLeft;
        else if (selectedSlotX == 2 && selectedSlotY == 1) sTemp = Player_Shooting.SlotName.Back;
        else if (selectedSlotX == 3 && selectedSlotY == 1) sTemp = Player_Shooting.SlotName.BackRight;
        else if (selectedSlotX == 1 && selectedSlotY == 2) sTemp = Player_Shooting.SlotName.FrontLeft;
        else if (selectedSlotX == 2 && selectedSlotY == 2) sTemp = Player_Shooting.SlotName.Front;
        else if (selectedSlotX == 3 && selectedSlotY == 2) sTemp = Player_Shooting.SlotName.FrontRight;

        foreach (KeyValuePair<Player_Shooting.SlotName, Player_Shooting.WeaponName> pair in PlayerShooting.currentWeaponsState)
        {
            if (sTemp == pair.Key)
            {
                if (pair.Value == Player_Shooting.WeaponName.BigFuckingGun ||
                pair.Value == Player_Shooting.WeaponName.Canon ||
                pair.Value == Player_Shooting.WeaponName.Mitraillette ||
                pair.Value == Player_Shooting.WeaponName.Shotgun)
                {
                    return true;
                }
            }


        }

        return false;

    }
}
