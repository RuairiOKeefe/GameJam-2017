using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTurret : Turret {
    public GameObject projectile;
    public Transform shotOrigin;

    protected override void AquireTarget()
    {
        base.AquireTarget();

        float timeToHit = Vector3.Distance(target.transform.position, transform.position) / projectile.GetComponent<Shot>().speed;

        Vector3 targetVelocity = target.GetComponent<Rigidbody2D>().velocity;

        Vector3 aimPoint = target.transform.position + (targetVelocity * timeToHit);

        Vector3 rotDir = (aimPoint - transform.position);
        float angle = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Fire()
    {
        Instantiate(projectile, shotOrigin.transform.position, this.transform.rotation);
        base.Fire();
    }
}
