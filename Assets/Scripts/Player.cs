using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private PlayerController playerController;
    private GunController gunController;

    private Camera viewCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
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
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.LookAt(point);
        }

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            gunController.Fire();
        }
    }
}
