using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool isDead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeHit(damage);
    }

    public virtual void TakeHit(float damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        isDead = true;
        if (OnDeath != null)
        {
            OnDeath(); // Fire event
        }
        Destroy(gameObject);
    }
}
