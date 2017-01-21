using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	// Declares the various game states
    public enum GameState
    {
        MainMenu, HighScores, Gameplay, Pause, Wiki
    }
	
	//Stores the current and previous state
    public GameState m_currState;
    public GameState m_oldState;
	
	// List of state objects that are to be activated by state manager
    public List<GameObject> m_stateObjects;

	// Use this for initialization
	void Start ()
    {
        m_currState = GameState.MainMenu;
	}
	
	// Update is called once per frame
	void Update ()
    {
		// Switch statemwnt checking what the current gamestate is, then
		// checking if player pressed escape. This will switch between pause and gameplay
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
	
	// Method for switching to a new state given and index for the state to be activated
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
	
	// Method to return to previous state (not tested!)
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
