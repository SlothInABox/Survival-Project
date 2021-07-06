using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    public Gun[] allGuns;
    private Gun equippedGun;

    private void Start()
    {

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

    public void EquipGun(int weaponIndex)
    {
        EquipGun(allGuns[weaponIndex]);
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerReleased()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerReleased();
        }
    }

    public float GunHeight
    {
        get
        {
            return weaponHold.position.y;
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (equippedGun != null)
        {
            equippedGun.Aim(aimPoint);
        }
    }

    public void Reload()
    {
        if (equippedGun != null)
        {
            equippedGun.Reload();
        }
    }
}
