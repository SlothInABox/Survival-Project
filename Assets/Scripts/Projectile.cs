using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_Speed = 10;
    public float Speed { get; set; }
    private float damage = 1;

    [SerializeField] private LayerMask collisionMask;

    // Update is called once per frame
    void Update()
    {
        float moveDistance = m_Speed * Time.deltaTime;
        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }

    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        Destroy(gameObject);
        
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
    }
}
