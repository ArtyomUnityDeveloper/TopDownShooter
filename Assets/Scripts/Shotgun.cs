using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] GameObject projectile;


    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    [SerializeField] Transform firePoint3;
    [SerializeField] Transform firePoint4;
    [SerializeField] Transform firePoint5;
    [SerializeField] Transform firePoint6;


    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        GameObject projectileUnit1 = Instantiate(projectile, firePoint1.position, firePoint1.rotation);
        GameObject projectileUnit2 = Instantiate(projectile, firePoint2.position, firePoint2.rotation);
        GameObject projectileUnit3 = Instantiate(projectile, firePoint3.position, firePoint3.rotation);
        GameObject projectileUnit4 = Instantiate(projectile, firePoint4.position, firePoint4.rotation);
        GameObject projectileUnit5 = Instantiate(projectile, firePoint5.position, firePoint5.rotation);
        GameObject projectileUnit6 = Instantiate(projectile, firePoint6.position, firePoint6.rotation);
    }
}
