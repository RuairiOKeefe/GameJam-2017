using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbMover : Mover {
    public Vector2 direction;

	// Use this for initialization
	void Start () {
        direction.Normalize();
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveHorizontal = direction.x;
            moveVertical = direction.y;
        }
        base.FixedUpdate();
    }
}
