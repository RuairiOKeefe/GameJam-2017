using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu, HighScores, Gameplay, Pause, Wiki
    }

    public GameState m_currState;
    public GameState m_oldState;

    public List<GameObject> m_stateObjects;

	// Use this for initialization
	void Start ()
    {
        m_currState = GameState.MainMenu;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch(m_currState)
        {
            case GameState.Gameplay:
                if (Input.GetButtonDown("Cancel"))
                    ChangeState(3);
                break;
            case GameState.Pause:
                if (Input.GetButtonDown("Cancel"))
                    ChangeState(2);
                break;
        }
	}

    public void ChangeState(int nextState)
    {
        m_oldState = m_currState;
        m_currState = (GameState)nextState;

        for (int i = 0; i < m_stateObjects.Count; i++)
        {
            if (i == nextState)
            {
                m_stateObjects[i].SetActive(true);
            }
            else
            {
                m_stateObjects[i].SetActive(false);
            }
        }
    }

    public void ReturnToPrevState()
    {
        GameState tmp = m_oldState;
        m_oldState = m_currState;
        m_currState = tmp;

        for (int i = 0; i < m_stateObjects.Count; i++)
        {
            if (i == (int)m_currState)
            {
                m_stateObjects[i].SetActive(true);
            }
            else
            {
                m_stateObjects[i].SetActive(false);
            }
        }
    }
}
