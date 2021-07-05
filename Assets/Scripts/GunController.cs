using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Gun startingGun;
    private Gun equippedGun;

    private void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gun)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gun, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.SetParent(weaponHold);
    }

    public void Fire()
    {
        if (equippedGun != null)
        {
            equippedGun.Fire();
        }
    }
}
