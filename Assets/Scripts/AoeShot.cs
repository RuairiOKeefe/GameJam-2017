using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeShot : Shot
{
    public Vector3 targetPos;
    public float SplashRadius;
    public float shotLife = 5.0f;

    private List<Collider2D> inRangeEnemies = new List<Collider2D>();

    protected override void Start()
    {
        GetComponent<CircleCollider2D>().radius = SplashRadius;
        GetComponent<Rigidbody2D>().velocity = (targetPos - transform.position).normalized * speed;
        shotLife = Time.time;
    }

    // Update is called once per frame
    void Update ()
    {
        // using .1 instead of float.Epsilon because float.Epsilon is being a dick
		if(Vector2.Distance(transform.position, targetPos) < .1f)
        {
            Splash();
        }

        if (shotLife <= 0.0f)
            Destroy(this.gameObject);
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (inRangeEnemies.Contains(other))
        {
            inRangeEnemies.Remove(other);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!inRangeEnemies.Contains(other) && other.gameObject.tag == "Enemy")
        {
            inRangeEnemies.Add(other);
        }
    }

    void Splash()
    {
        foreach(Collider2D hitEnemy in inRangeEnemies)
        {
            hitEnemy.gameObject.GetComponent<Health>().TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }
}
