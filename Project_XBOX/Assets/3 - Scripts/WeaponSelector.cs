using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public Sprite[] icons;
    public Image iconWeapon01;
    public Image iconWeapon02;
    public TextMeshProUGUI txtWeapon01;
    public TextMeshProUGUI txtWeapon02;
    public Image[] iconWeaponSlot;
    public Transform slots;

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


        if(firstMenu.activeSelf)
        {
            HandleFirstMenuInputs();
        }

        if(mainMenu.activeSelf)
        {
            HandleMainMenuInputs();
        }
    }

    public void ChooseNewItem()
    {
        if(firstMenu.activeSelf)
        {
            return;
        }

        SetStates(true, false);

        GenerateTwoAmongEverything();
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

        selectedId = 0;

        ShowWeaponOnSlot();
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

            ShowWeaponSelection(tempWeapon_1.ToString(), tempUpgrade_1.ToString());
        }
    }

    IEnumerator IGenerateWeapons()
    {
        tempWeapon_1 = GetRandomWeapon();
        tempWeapon_2 = GetRandomWeapon();

        while (tempWeapon_1 == tempWeapon_2)
        {
            tempWeapon_2 = GetRandomWeapon();
            yield return null;
        }

        print($"1 : {tempWeapon_1} - 2 : {tempWeapon_2}");

        ShowWeaponSelection(tempWeapon_1.ToString(), tempWeapon_2.ToString());
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

        ShowWeaponSelection(tempUpgrade_1.ToString(), tempUpgrade_2.ToString());
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

    private void ShowWeaponSelection(string _weapon01, string _weapon02)
    {
        if(_weapon01 == "Canon (Canon)")
        {
            txtWeapon01.text = "Cannon";
            iconWeapon01.sprite = icons[0];
        }
        else if (_weapon01 == "BigFuckingGun (BigFuckingGun)")
        {
            txtWeapon01.text = "BigFuckingGun";
            iconWeapon01.sprite = icons[1];
        }
        else if (_weapon01 == "Mitraillette (Mitraillette)")
        {
            txtWeapon01.text = "Magic Gun";
            iconWeapon01.sprite = icons[2];
        }
        else if (_weapon01 == "FusilPompe (FusilPompe)")
        {
            txtWeapon01.text = "Shotgun";
            iconWeapon01.sprite = icons[3];
        }
        else if (_weapon01 == "Propulseur (Propulseur)")
        {
            txtWeapon01.text = "Propulsor";
            iconWeapon01.sprite = icons[4];
        }
        else if (_weapon01 == "Shield (Shield)")
        {
            txtWeapon01.text = "Shield";
            iconWeapon01.sprite = icons[5];
        }
        else if (_weapon01 == "Dash (Dash)")
        {
            txtWeapon01.text = "Dash";
            iconWeapon01.sprite = icons[6];
        }
        else if (_weapon01 == "Totems (Totems)")
        {
            txtWeapon01.text = "Totems";
            iconWeapon01.sprite = icons[7];
        }

        if (_weapon02 == "Canon (Canon)")
        {
            txtWeapon02.text = "Cannon";
            iconWeapon02.sprite = icons[0];
        }
        else if (_weapon02 == "BigFuckingGun (BigFuckingGun)")
        {
            txtWeapon02.text = "BigFuckingGun";
            iconWeapon02.sprite = icons[1];
        }
        else if (_weapon02 == "Mitraillette (Mitraillette)")
        {
            txtWeapon02.text = "Magic Gun";
            iconWeapon02.sprite = icons[2];
        }
        else if (_weapon02 == "FusilPompe (FusilPompe)")
        {
            txtWeapon02.text = "Shotgun";
            iconWeapon02.sprite = icons[3];
        }
        else if (_weapon02 == "Propulseur (Propulseur)")
        {
            txtWeapon02.text = "Propulsor";
            iconWeapon02.sprite = icons[4];
        }
        else if (_weapon02 == "Shield (Shield)")
        {
            txtWeapon02.text = "Shield";
            iconWeapon02.sprite = icons[5];
        }
        else if (_weapon02 == "Dash (Dash)")
        {
            txtWeapon02.text = "Dash";
            iconWeapon02.sprite = icons[6];
        }
        else if (_weapon02 == "Totems (Totems)")
        {
            txtWeapon02.text = "Totems";
            iconWeapon02.sprite = icons[7];
        }

        SetSelector(SelectorRotation.Left);
    }

    private void ShowWeaponOnSlot()
    {
        for(int i = 0; i < slots.childCount; i++)
        {
            if(slots.GetChild(i).childCount == 1)
            {
                if (slots.GetChild(i).GetChild(0).name == "Canon(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[0];
                }
                else if (slots.GetChild(i).GetChild(0).name == "BigFuckingGun(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[1];
                }
                else if (slots.GetChild(i).GetChild(0).name == "Mitraillette(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[2];
                }
                else if (slots.GetChild(i).GetChild(0).name == "FusilPompe(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[3];
                }
                else if (slots.GetChild(i).GetChild(0).name == "Propulseur(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[4];
                }
                else if (slots.GetChild(i).GetChild(0).name == "Shield(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[5];
                }
                else if (slots.GetChild(i).GetChild(0).name == "Dash(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[6];
                }
                else if (slots.GetChild(i).GetChild(0).name == "Totems(Clone)")
                {
                    iconWeaponSlot[i].sprite = icons[7];
                }
            }
        }
    }

    public enum SelectorRotation { Left, Right, HautDroite, HautGauche, BasDroite, BasGauche, Haut, Bas };
    private int selectedId = 0;
    public RectTransform selector;
    public RectTransform selector2;

    //-122/122
    private void SetSelector(SelectorRotation rot)
    {
        if (firstMenu.activeSelf) { selector.gameObject.SetActive(true); selector2.gameObject.SetActive(false); }
        else if (mainMenu.activeSelf) { selector2.gameObject.SetActive(true); selector.gameObject.SetActive(false); }


        selectedId = 0;//Selectionne le choix de gauche par défaut

        selector.localPosition = new Vector2(0, 0);
        selector2.localPosition = new Vector2(0, -70);

        if(firstMenu.activeSelf)
        {
            if (rot == SelectorRotation.Left)
                selector.localRotation = Quaternion.Euler(0, 0, 180);

            if (rot == SelectorRotation.Right)
                selector.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if(mainMenu.activeSelf)
        {
            if (rot == SelectorRotation.Haut) selector2.localRotation = Quaternion.Euler(0, 0, 90);
            if (rot == SelectorRotation.Bas) selector2.localRotation = Quaternion.Euler(0, 0, -90);
            if (rot == SelectorRotation.HautDroite) selector2.localRotation = Quaternion.Euler(0, 0, 30);
            if (rot == SelectorRotation.BasDroite) selector2.localRotation = Quaternion.Euler(0, 0, -30);
            if (rot == SelectorRotation.HautGauche) selector2.localRotation = Quaternion.Euler(0, 0, -210);
            if (rot == SelectorRotation.BasGauche) selector2.localRotation = Quaternion.Euler(0, 0, -150);
        }

    }        //90 Haut
             //30 haut droite
             //-30 bas droite
             //-90 bas 
             //-150 bas gauche
             //-210 haut gauche

    private void HandleFirstMenuInputs()
    {
        //float x = Input.GetAxisRaw("Input_Rotation_Controller_Horizontal");//+ gérer retour joystick au centre
        float x = Input.GetAxisRaw("Horizontal");

        if (x > 0) { SetSelector(SelectorRotation.Right); selectedId = 0; } 
        if (x < 0) { SetSelector(SelectorRotation.Left); selectedId = 1; }


        if(Input.GetKeyDown(KeyCode.F))//ou input A sur manette
        {
            switch (selectedId)
            {
                case 0:
                    SetWeaponToButton(0);
                    break;
                case 1:
                    SetWeaponToButton(1);
                    break;
            }
        }
        
    
    }

    private void HandleMainMenuInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //print($"x: {x} / y: {y}");

        if (x == 1 && y == 1) { SetSelector(SelectorRotation.HautDroite); selectedId = 0; }
        if (x == 1 && y == -1) { SetSelector(SelectorRotation.BasDroite); selectedId = 1; }
        if (x == 0 && y == -1) { SetSelector(SelectorRotation.Bas); selectedId = 2; }
        if (x == -1 && y == -1) { SetSelector(SelectorRotation.BasGauche); selectedId = 3; }
        if (x == -1 && y == 1) { SetSelector(SelectorRotation.HautGauche); selectedId = 4; }
        if (x == 0 && y == 1) { SetSelector(SelectorRotation.Haut); selectedId = 5; }

        if (Input.GetKeyDown(KeyCode.F) && mainMenu.activeSelf && selectedId != -1)//ou input A sur manette
        {
            switch (selectedId)
            {
                case 0:
                    SetWeaponOnSlot("FrontRight");
                    break;
                case 1:
                    SetWeaponOnSlot("BackRight");
                    break;
                case 2:
                    SetWeaponOnSlot("Back");
                    break;
                case 3:
                    SetWeaponOnSlot("BackLeft");
                    break;
                case 4:
                    SetWeaponOnSlot("FrontLeft");
                    break;
                case 5:
                    SetWeaponOnSlot("Front");
                    break;
            }
        }


    }
}
