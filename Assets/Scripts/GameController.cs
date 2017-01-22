using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public int Lives;
    [SerializeField]
    public int Money;

    private int lives_;
    private int money_;
    private bool paused;

	// Use this for initialization
	void Start ()
    {
        lives_ = Lives;
        money_ = Money;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public int GetLives()
    {
        return lives_;
    }

    public int GetMoney()
    {
        return money_;
    }

    public void SetLives(int lives)
    {
        lives_ -= lives;
    }

    public void SetMoney(int money)
    {
        money_ += money;
    }
}
