using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single };
    [SerializeField] FireMode fireMode;

    [SerializeField] private Transform[] projectileSpawns;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float msBetweenShots = 100;
    [SerializeField] private float muzzleVelocity = 35;
    [SerializeField] private int burstCount;

    [SerializeField] private Transform shellPrefab;
    [SerializeField] private Transform shellEjection;
    private MuzzleFlash muzzleFlash;

    private float nextShotTime;

    private bool triggerReleasedSinceLastShot;
    private int shotsRemainingInBurst;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
    }

    private void Fire()
    {
        if (Time.time > nextShotTime)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            foreach (Transform projectileSpawn in projectileSpawns)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
                newProjectile.Speed = muzzleVelocity;

            }

            Instantiate(shellPrefab, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
        }
    }

    public void OnTriggerHold()
    {
        Fire();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerReleased()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
