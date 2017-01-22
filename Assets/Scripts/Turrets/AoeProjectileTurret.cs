using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeProjectileTurret : ProjectileTurret {
    protected override void AquireTarget()
    {
        base.AquireTarget();
        Vector3 rotDir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Fire()
    {
        projectile.GetComponent<AoeShot>().targetPos = target.transform.position;
        base.Fire();
    }
}
