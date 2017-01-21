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

            Vector3 knockBackVec = (enemy.transform.position - transform.position).normalized * KnockbackStrength;

            // TODO: decide how we will actually be handling this
            enemy.gameObject.GetComponent<Mover>().Move(knockBackVec.x, knockBackVec.y);
        }

        base.Fire();
    }
}
