using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {

    public float range;
    public float fireRate;
    public GameObject projectile;
    public Transform shotOrigin;
    
    private float nextFire;
    private List<Collider2D> HostileList = new List<Collider2D>();
    private Transform target;

	void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        if (HostileList.Count != 0)
        {
            AquireTarget();

            //Make it follow a target
            if (Time.time >= nextFire)
            {
                Fire();
            }
        }


		
	}

    void AquireTarget()
    {
        Collider2D selected = null;
        float minDist = float.PositiveInfinity;
        foreach (Collider2D enemy in HostileList)
        {
            if (enemy.GetComponentInParent<WalkerAI>().distToEnd < minDist)
            {
                selected = enemy;
                minDist = enemy.GetComponentInParent<WalkerAI>().distToEnd;
            }
        }
        target = selected.transform;
        Vector3 rotDir = target.position - transform.position;
        float angle = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Fire()
    {
        Instantiate(projectile, shotOrigin.transform.position, gameObject.transform.rotation);
        nextFire = Time.time + fireRate;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        print("Arse");
        if (!HostileList.Contains(other) && other.tag == "Enemy")
        {
            HostileList.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        print("De-Arse");
        if (HostileList.Contains(other) && other.tag == "Enemy")
        {
            HostileList.Remove(other);
        }
    }
}
