using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shot : MonoBehaviour {

    public float speed;
    public float damage;
    public int penetrationFactor;

	protected virtual void Start ()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
	}

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Health>().TakeDamage(damage);
            //play sound
            --penetrationFactor;
            if(penetrationFactor <= 0)
                Destroy(this.gameObject);
        }
    }
}
