using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{

    public float maxHealth;
    public float currentHealth;
    public Transform end;
    public float distToEnd;

    protected bool isStunned;
    protected float stunOver;

    private bool isDead;

    public GameObject LevelMap;

    public List<GameObject> enemyList;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void GetStunned(float stunTime)
    {
        isStunned = true;
        stunOver = Time.time + stunTime;
    }

    void Death()
    {
        //if (this.tag == "Player")
        //{
        //    isDead = true;
        //    //this.GetComponent<PlayerMovement>().enabled = false;
        //    GetComponent<Rigidbody>().MovePosition(transform.position);
        //}
        //else if (this.tag == "Enemy")
        //{
        //    Destroy(gameObject);
        //    enemyList.Remove(gameObject);
        //}

        if (gameObject.tag == "Castle")
        {
            LevelManager man = LevelMap.GetComponent<LevelManager>();
            man.IsGameOver = true;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
            enemyList.Remove(gameObject);
        }

        

    }

    void OnTriggerEnter2D(Collider2D en)
    {
        if (this.tag == "Castle"  && en.tag == "Enemy")
        {
            this.TakeDamage(10);
            Destroy(en.gameObject);
        }
    }
}