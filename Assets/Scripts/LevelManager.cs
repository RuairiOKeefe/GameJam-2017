using UnityEngine;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] tilePrefabs;

    public float TileSize
    {
        // Getter for tileSize
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

	// Use this for initialization
	void Start ()
    {
        CreateLevel();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void CreateLevel()
    {
        // Load in map data as a string array - see method for more info
        string[] mapData = ReadLevelText();

        // Sets the map X and Y sizes
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        // Initialized the world start set to the camera position

        // TODO - need to initialize based on a fixed camera size for level 1 which doubles the X and Y ranges (zooms out) for levels 2+
        print(mapYSize);
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, mapYSize));

        for(int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, mapYSize-1);
            }
        }
    }

    private void PlaceTile(string tileType, int x, int y, float yStart)
    {
        // Places a tile based on the prefab index of the Level Manager. Index is parsed as an Integer from the text file [0 for beach, 1 for trench, etc]

        // Get next tile index
        int tileIndex = int.Parse(tileType);

        // Instantiate new tile object
        //GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        Instantiate(tilePrefabs[tileIndex], new Vector3(x + 0.5f, (yStart - y) + 0.5f, 1.0f), Quaternion.identity);

        // Move current tile object by the proper 2D array space
        //newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
    }

    private string[] ReadLevelText()
    {
        // Currently loading level-1 as a default. Will need to be changed to allow for multiple loading of files given level progression.

        // Bind the txt file as data as a text asset
        TextAsset bindData = Resources.Load("Level-1") as TextAsset;

        print(bindData);

        // Parse the string of tileIntegers from the text asset
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        // Return the entire string array, splitting indexes by the '-' mark [ mark signifies new row of tiles ]
        return data.Split('-');
    }
}
