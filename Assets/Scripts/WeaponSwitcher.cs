using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int currentWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        ChooseWeapon();
    }



    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = currentWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)
            { currentWeapon = 0; }
            else
            { currentWeapon++; }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon <= 0)
            { currentWeapon = transform.childCount - 1; }
            else
            { currentWeapon--; }
        }

        if (previousSelectedWeapon != currentWeapon)
        {
            ChooseWeapon();
        }
    }

    private void ChooseWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon) { weapon.gameObject.SetActive(true); }
            else { weapon.gameObject.SetActive(false); }
            i++;
        }
    }
}
