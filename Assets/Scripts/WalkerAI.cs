using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAI : Health{



    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
		distToEnd = (transform.position - end.position).sqrMagnitude;
	}
}
