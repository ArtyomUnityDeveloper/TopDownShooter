using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject projectile;


    private Transform firePoint;


    private void Update()
    {
        firePoint = gameObject.transform;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        GameObject projectileUnit = Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}
