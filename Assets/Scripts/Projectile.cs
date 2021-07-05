using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_Speed = 10;
    public float Speed { get; set; }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }
}
