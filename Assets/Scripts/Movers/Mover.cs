using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour {
    public float speed;

    protected float moveHorizontal;
    protected float moveVertical;

    // Use this for initialization
    void Start () {
		
	}
	
	protected virtual void FixedUpdate ()
    {
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            Move(moveHorizontal, moveVertical);
        }
    }

    void Move(float moveHorizontal, float moveVertical)
    {
        //May need to make sure that diagonal movement isnt faster, normalization doesnt work as it causes player to drift to a hault - ruins feel of movement, player always feel in control.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        GetComponent<Rigidbody2D>().velocity = movement * speed;
    }
}
