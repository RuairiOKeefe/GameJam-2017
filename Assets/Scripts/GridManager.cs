using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public int columns;
    public int rows;

    public GameObject sand;
    public GameObject trench;
    public GameObject rock;

    void Start()
    {
        GridSetup();
    }

    void GridSetup()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if(y == 2)
                    Instantiate(trench, new Vector3(x + 0.5f, y + 0.5f, 1.0f), Quaternion.identity);
                else
                    if (y == 4)
                        Instantiate(rock, new Vector3(x + 0.5f, y + 0.5f, 1.0f), Quaternion.identity);
                    else
                        Instantiate(sand, new Vector3(x + 0.5f, y + 0.5f, 1.0f), Quaternion.identity);
            }
        }
    }


}
