using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponSelectionMenu;

    private Player_Shooting ps;

    private void Awake()
    {
        ps = GameObject.Find("Player").GetComponent<Player_Shooting>();

        weaponSelectionMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool newState = !weaponSelectionMenu.activeSelf;
            weaponSelectionMenu.SetActive(newState);

            if (newState) Time.timeScale = 0;
            if (!newState) Time.timeScale = 1;


        }
    }


    //Appliquer l'arme à emplacement choisi
    public void SetWeaponOnSlot(string slot)
    {
        Player_Shooting.SlotName enumState = (Player_Shooting.SlotName)Player_Shooting.SlotName.Parse(typeof(Player_Shooting.SlotName), slot);

        ps.SetWeapon(enumState, Player_Shooting.WeaponName.Canon);
    }
}
