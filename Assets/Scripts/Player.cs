using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    [SerializeField] private float speed = 5;

    [SerializeField] private Crosshairs crosshairs;
    private PlayerController playerController;
    private GunController gunController;

    private Camera viewCamera;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
        FindObjectOfType<SpawnManager>().OnNewWave += OnNewWave;
    }

    private void OnNewWave(int waveNumber)
    {
        health = startingHealth;
        gunController.EquipGun(waveNumber - 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = moveInput.normalized * speed;
        playerController.Move(velocity);

        // Look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.LookAt(point);
            crosshairs.transform.position = point;
            crosshairs.DetectTargets(ray);
            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
            {
                gunController.Aim(point);
            }
        }

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerReleased();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }
    }

    public override void Die()
    {
        AudioManager.Instance.PlaySound("Player Death", transform.position);
        base.Die();
    }
}
