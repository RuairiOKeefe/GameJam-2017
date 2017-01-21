using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBMover : Mover {
    // Use this for initialization
    void Start () {
		
	}
	
	protected override void FixedUpdate ()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        base.FixedUpdate();
    }
}
