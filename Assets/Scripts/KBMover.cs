using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBMover : MonoBehaviour {

    public float speed;

    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Move(moveHorizontal, moveVertical);
    }

    void Move(float moveHorizontal, float moveVertical)
    {
        //May need to make sure that diagonal movement isnt faster, normalization doesnt work as it causes player to drift to a hault - ruins feel of movement, player always feel in control.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;
    }
}
