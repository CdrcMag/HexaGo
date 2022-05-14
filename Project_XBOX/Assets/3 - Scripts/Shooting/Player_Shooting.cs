using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    [Header("Start settings")]
    public WeaponName FrontStartWeapon;
    public WeaponName FrontRightStartWeapon;
    public WeaponName FrontLeftStartWeapon;

    public UpgradeName BackStartUpgrade;
    public UpgradeName BackRightStartUpgrade;
    public UpgradeName BackLeftStartUpgrade;

    [Header("Front weapons")]
    public Weapon[] ArmesFront = new Weapon[3];
    public Upgrade[] Upgrades = new Upgrade[1];

    [Header("Slots")]
    //Répertorie tous les transform des slots
    public Transform slotFront;
    public Transform slotBack;
    public Transform slotFrontRight;
    public Transform slotFrontLeft;
    public Transform slotBackRight;
    public Transform slotBackLeft;

    public Dictionary<SlotName, WeaponName> currentWeaponsState = new Dictionary<SlotName, WeaponName>();
    public Dictionary<SlotName, UpgradeName> currentUpgradesState = new Dictionary<SlotName, UpgradeName>();


    //Nom des slots et noms des armes
    public enum SlotName { None, Front, Back, FrontRight, FrontLeft, BackRight, BackLeft };
    public enum WeaponName { None = 0, Canon = 1, BigFuckingGun = 2, Mitraillette = 3, Shotgun = 4, Drone = 5, Croissant = 6, LaTortuga = 7 };
    public enum UpgradeName { None, Propulseur, Dash, Totems, Shield };

    private void Start()
    {
        //Assigns weapons at start
        SetWeapon(SlotName.Front, FrontStartWeapon);
        SetWeapon(SlotName.FrontLeft, FrontLeftStartWeapon);
        SetWeapon(SlotName.FrontRight, FrontRightStartWeapon);

        SetUpgrade(SlotName.Back, BackStartUpgrade);
        SetUpgrade(SlotName.BackLeft, BackLeftStartUpgrade);
        SetUpgrade(SlotName.BackRight, BackRightStartUpgrade);

    }

    //Assigne une arme à un slot
    public void SetWeapon(SlotName slotName, WeaponName weaponName)//Appel exemple : this.SetWeapon(SlotName.FrontLeft, WeaponName.BigFuckingGun);
    {
        Weapon currentWeapon = GetWeapon(weaponName);
        Transform currentSlot = GetSlot(slotName);

        //if no weapons are selected
        if (currentWeapon == null)
            return;

        if (currentSlot.childCount != 0)
            Destroy(currentSlot.GetChild(0).gameObject);

        //Copies the weapon from the storage
        GameObject weaponInstance = Instantiate(currentWeapon.gameObject, currentSlot.position, currentSlot.rotation, currentSlot);

        //And activates it
        weaponInstance.SetActive(true);

        //Updates the dictionnary
        if(currentWeaponsState.ContainsKey(slotName))
            currentWeaponsState.Remove(slotName);

        currentWeaponsState.Add(slotName, weaponName);
 

    }

    public void SetWeapon(SlotName slotName, Weapon w)
    {
        Transform currentSlot = GetSlot(slotName);

        if (currentSlot.childCount != 0)
            Destroy(currentSlot.GetChild(0).gameObject);

        //Copies the weapon from the storage
        GameObject weaponInstance = Instantiate(w.gameObject, currentSlot.position, currentSlot.rotation, currentSlot);

        //And activates it
        weaponInstance.SetActive(true);

        //Updates the dictionnary
        if (currentWeaponsState.ContainsKey(slotName))
            currentWeaponsState.Remove(slotName);

        if (w.GetType() == typeof(Canon)) currentWeaponsState.Add(slotName, WeaponName.Canon);
        if (w.GetType() == typeof(BigFuckingGun)) currentWeaponsState.Add(slotName, WeaponName.BigFuckingGun);
        if (w.GetType() == typeof(Mitraillette)) currentWeaponsState.Add(slotName, WeaponName.Mitraillette);
        if (w.GetType() == typeof(FusilPompe)) currentWeaponsState.Add(slotName, WeaponName.Shotgun);
        if (w.GetType() == typeof(Drone)) currentWeaponsState.Add(slotName, WeaponName.Drone);
        if (w.GetType() == typeof(CroissantDeLune)) currentWeaponsState.Add(slotName, WeaponName.Croissant);
        if (w.GetType() == typeof(LaTortuga)) currentWeaponsState.Add(slotName, WeaponName.LaTortuga);
        //ici rajouter des armes en suivant la structure ci-dessus


    }

    public void SetUpgrade(SlotName slotName, UpgradeName upgrade)
    {
        Upgrade currentUpgrade = GetUpgrade(upgrade);
        Transform currentSlot = GetSlot(slotName);

        //if no weapons are selected
        if (currentUpgrade == null)
            return;

        if (currentSlot.childCount != 0)
            Destroy(currentSlot.GetChild(0).gameObject);

        //Copies the weapon from the storage
        GameObject upgradeInstance = Instantiate(currentUpgrade.gameObject, transform.position, Quaternion.identity);

        //Sets position and rotation
        upgradeInstance.transform.localRotation = currentSlot.localRotation;
        upgradeInstance.transform.localPosition = currentSlot.localPosition;

        //Assigns the weapon to the parent
        upgradeInstance.transform.SetParent(currentSlot);

        //And activates it
        upgradeInstance.SetActive(true);

        //Updates the dictionnary
        if (currentUpgradesState.ContainsKey(slotName))
            currentUpgradesState.Remove(slotName);

        currentUpgradesState.Add(slotName, upgrade);

    }

    public void SetUpgrade(SlotName slotName, Upgrade u)
    {
        Transform currentSlot = GetSlot(slotName);

        if (currentSlot.childCount != 0)
            Destroy(currentSlot.GetChild(0).gameObject);

        //Copies the weapon from the storage
        GameObject upgradeInstance = Instantiate(u.gameObject, transform.position, Quaternion.identity);

        //Sets position and rotation
        upgradeInstance.transform.localRotation = currentSlot.localRotation;
        upgradeInstance.transform.localPosition = currentSlot.localPosition;

        //Assigns the weapon to the parent
        upgradeInstance.transform.SetParent(currentSlot);

        //And activates it
        upgradeInstance.SetActive(true);

        //Updates the dictionnary
        if (currentUpgradesState.ContainsKey(slotName))
            currentUpgradesState.Remove(slotName);

        if (u.GetType() == typeof(Dash)) currentUpgradesState.Add(slotName, UpgradeName.Dash);
        if (u.GetType() == typeof(Propulseur)) currentUpgradesState.Add(slotName, UpgradeName.Propulseur);
        if (u.GetType() == typeof(Shield)) currentUpgradesState.Add(slotName, UpgradeName.Shield);
        if (u.GetType() == typeof(Totems)) currentUpgradesState.Add(slotName, UpgradeName.Totems);
    }

    private Transform GetSlot(SlotName s)    
    {
        switch (s)
        {
            case SlotName.Front:
                return slotFront;
            case SlotName.Back:
                return slotBack;
            case SlotName.FrontRight:
                return slotFrontRight;
            case SlotName.FrontLeft:
                return slotFrontLeft;
            case SlotName.BackRight:
                return slotBackRight;
            case SlotName.BackLeft:
                return slotBackLeft;

            default:
                return null;
        }
    }

    public Weapon GetWeapon(WeaponName w)
    {

        switch (w)
        {
            case WeaponName.Shotgun:
                return ArmesFront[0];//Fusil à pompe
            case WeaponName.Canon:
                return ArmesFront[1];//Canon
            case WeaponName.Mitraillette:
                return ArmesFront[2];//Mitraillette
            case WeaponName.BigFuckingGun:
                return ArmesFront[3];//Big fucking gun
            //Ici rajouter votre arme
            case WeaponName.Drone:
                return ArmesFront[4];
            case WeaponName.Croissant:
                return ArmesFront[5];
            case WeaponName.LaTortuga:
                return ArmesFront[6];

            default:
                return null;
        }
    }

    private Upgrade GetUpgrade(UpgradeName u)
    {
        switch(u)
        {
            case UpgradeName.Propulseur:
                return Upgrades[0];
            case UpgradeName.Dash:
                return Upgrades[1];
            case UpgradeName.Totems:
                return Upgrades[2];
            case UpgradeName.Shield:
                return Upgrades[3];

            default:
                break;
        }

        return null;
    }

    public string GetWeaponOnSlot(SlotName s)
    {
        string final = "";

        if (currentWeaponsState.ContainsKey(s))
        {
            if (currentWeaponsState[s] == WeaponName.Canon)  return "Canon"; // return (Canon)final;
            if (currentWeaponsState[s] == WeaponName.BigFuckingGun) return  "BFG";
            if (currentWeaponsState[s] == WeaponName.Mitraillette) return "Mitraillette";
            if (currentWeaponsState[s] == WeaponName.Shotgun) return "Shotgun";
            if (currentWeaponsState[s] == WeaponName.Drone) return "Drone";
            if (currentWeaponsState[s] == WeaponName.Croissant) return "Croissant";
            if (currentWeaponsState[s] == WeaponName.LaTortuga) return "Tortuga";
        }

        if (currentUpgradesState.ContainsKey(s))
        {
            if (currentUpgradesState[s] == UpgradeName.Dash) return "Dash";
            if (currentUpgradesState[s] == UpgradeName.Propulseur) return "Propulseur";
            if (currentUpgradesState[s] == UpgradeName.Shield) return "Shield";
            if (currentUpgradesState[s] == UpgradeName.Totems) return "Totems";

        }

        return final;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            print("============ ARMES ============");
            foreach (SlotName n in currentWeaponsState.Keys)
            {
                print($"{n} a l'arme {currentWeaponsState[n]}");
            }
            print("============UPGRADES ============");
            foreach (SlotName n in currentUpgradesState.Keys)
            {
                print($"{n} a l'upgrade {currentUpgradesState[n]}");
            }
        }
    }



}
