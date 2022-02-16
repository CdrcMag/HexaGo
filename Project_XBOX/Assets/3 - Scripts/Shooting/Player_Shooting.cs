using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    [Header("Start settings")]
    public WeaponName FrontStartWeapon;
    public WeaponName BackStartWeapon;
    public WeaponName FrontRightStartWeapon;
    public WeaponName FrontLeftStartWeapon;
    public WeaponName BackRightStartWeapon;
    public WeaponName BackLeftStartWeapon;

    [Header("Front weapons")]
    public Weapon[] ArmesFront = new Weapon[3];

    [Header("Slots")]
    //Répertorie tous les transform des slots
    public Transform slotFront;
    public Transform slotBack;
    public Transform slotFrontRight;
    public Transform slotFrontLeft;
    public Transform slotBackRight;
    public Transform slotBackLeft;


    //Nom des slots et noms des armes
    public enum SlotName { Front, Back, FrontRight, FrontLeft, BackRight, BackLeft };
    public enum WeaponName { None, Canon, BigFuckingGun, Mitraillette, Shotgun };

    private void Start()
    {
        //Assigns weapons at start
        SetWeapon(SlotName.Front, FrontStartWeapon);
        SetWeapon(SlotName.Back, BackStartWeapon);
        SetWeapon(SlotName.FrontLeft, FrontLeftStartWeapon);
        SetWeapon(SlotName.FrontRight, FrontRightStartWeapon);
        SetWeapon(SlotName.BackLeft, BackLeftStartWeapon);
        SetWeapon(SlotName.BackRight, BackRightStartWeapon);
    }

    //Assigne une arme à un slot
    public void SetWeapon(SlotName slotName, WeaponName weaponName)//Appel exemple : this.SetWeapon(SlotName.FrontLeft, WeaponName.BigFuckingGun);
    {
        Weapon currentWeapon = GetWeapon(weaponName);
        Transform currentSlot = GetSlot(slotName);

        //if no weapons are selected
        if (currentWeapon == null)
            return;

        //Copies the weapon from the storage
        GameObject weaponInstance = Instantiate(currentWeapon.gameObject, transform.position, Quaternion.identity);

        //Sets position and rotation
        weaponInstance.transform.localRotation = currentSlot.localRotation;
        weaponInstance.transform.localPosition = currentSlot.localPosition;

        //Assigns the weapon to the parent
        weaponInstance.transform.SetParent(currentSlot);

        //And activates it
        weaponInstance.SetActive(true);
        

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
                return slotBackRight;

            default:
                return null;
        }
    }

    private Weapon GetWeapon(WeaponName w)
    {

        switch (w)
        {
            case WeaponName.Shotgun:
                return ArmesFront[0];//Shotgun

            default:
                return null;
        }
    }


}
