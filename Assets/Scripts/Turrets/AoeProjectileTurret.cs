using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AoeProjectileTurret : ProjectileTurret {
    Vector3 aimPoint;

    protected override void AquireTarget()
    {
        base.AquireTarget();

        float timeToHit = Vector3.Distance(target.transform.position, transform.position) / projectile.GetComponent<Shot>().speed;

        Vector3 targetVelocity = target.GetComponent<Rigidbody2D>().velocity;

        // add a small margin of error
        aimPoint = target.transform.position + (targetVelocity * timeToHit) + new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0);

        Vector3 rotDir = (aimPoint - transform.position);

        float angle = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Fire()
    {
        projectile.GetComponent<AoeShot>().targetPos = aimPoint;
        base.Fire();
    }
}
