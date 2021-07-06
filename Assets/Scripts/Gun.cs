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
    [SerializeField] private int projectilesPerMag;
    [SerializeField] private float reloadTime = 0.3f;

    [Header("Recoil")]
    [SerializeField] private Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
    [SerializeField] private Vector2 recoilAngleMinMax = new Vector2(3, 5);
    [SerializeField] private float recoilMoveSettleTime = 0.1f;
    [SerializeField] private float recoilRotationSettleTime = 0.1f;

    [Header("Effects")]
    [SerializeField] private Transform shellPrefab;
    [SerializeField] private Transform shellEjection;
    private MuzzleFlash muzzleFlash;

    private float nextShotTime;

    private bool triggerReleasedSinceLastShot;
    private int shotsRemainingInBurst;
    private int projectilesRemainingInMag;
    private bool isReloading;

    private Vector3 recoilSmoothDampVelocity;
    private float recoilRotSmoothDampVelocity;
    private float recoilAngle;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMag = projectilesPerMag;
    }

    private void LateUpdate()
    {
        // Animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }

    private void Fire()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
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
                if (projectilesRemainingInMag == 0)
                {
                    break;
                }
                projectilesRemainingInMag--;
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
                newProjectile.Speed = muzzleVelocity;

            }

            Instantiate(shellPrefab, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    private IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRotation = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
    }

    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            transform.LookAt(aimPoint);
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
