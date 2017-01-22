using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour {
    public float range;
    public float fireRate;

    protected List<Collider2D> HostileList = new List<Collider2D>();
    protected GameObject target;
    protected float nextFire;
    protected Animator animator;

    void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = range;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (HostileList.Count != 0)
        {
            AquireTarget();

            //Make it follow a target
            if (Time.time >= nextFire)
            {
                animator.SetTrigger("Fire");
                Fire();
            }
        }
    }

    // TODO: UI element for setting turret targetting behaviour?
    protected virtual void AquireTarget()
    {
        Collider2D selected = null;
        float minDist = float.PositiveInfinity;
        foreach (Collider2D enemy in HostileList)
        {
            if (enemy == null)
            {
                HostileList.Remove(enemy);
            }
            if (enemy.GetComponentInParent<Health>().distToEnd < minDist)
            {
                selected = enemy;
                minDist = enemy.GetComponentInParent<Health>().distToEnd;
            }
        }
        target = selected.gameObject;
    }

    protected virtual void Fire()
    {
        nextFire = Time.time + fireRate;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!HostileList.Contains(other) && other.tag == "Enemy")
        {
            HostileList.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (HostileList.Contains(other) && other.tag == "Enemy")
        {
            HostileList.Remove(other);
        }
    }
}
