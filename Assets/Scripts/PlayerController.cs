using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 correctedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(correctedPoint);
    }

    void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + velocity * Time.deltaTime);
    }
}
