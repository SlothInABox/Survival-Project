using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float msBetweenShots = 100;
    [SerializeField] private float muzzleVelocity = 35;

    private float nextShotTime;

    public void Fire()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
            newProjectile.Speed = muzzleVelocity;
        }
    }
}
