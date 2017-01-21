using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour {

    public float speed;
    public float damage;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
