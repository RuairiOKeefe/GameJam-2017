using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShot : Shot {

	protected override void Start () {
        base.Start();
	}

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
