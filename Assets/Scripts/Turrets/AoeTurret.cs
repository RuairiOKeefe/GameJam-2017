using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTurret : Turret {
    public float Damage;
    public float KnockbackStrength;

    // Use this for initialization
    protected override void Fire()
    {
        foreach (Collider2D enemy in HostileList)
        {
            enemy.gameObject.GetComponent<Health>().TakeDamage(Damage);
        }

        base.Fire();
    }
}
