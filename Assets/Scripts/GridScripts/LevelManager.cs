using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    public String level;

    // Stores all map tiles
    public GameObject[,] tiles;

    // barrier prefab used for invisible projectile wall
    public GameObject barrier;

    public GameObject mover;
    public Transform start;

    // Stores map dimesnions
    public int mapSizeX;
    public int mapSizeY;

    public float TileSize
    {
        // Getter for tileSize
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

	// Use this for initialization
	void Start ()
    {
        print(level);
        mover.GetComponent<Health>().maxHealth = 100;
        Instantiate(mover, start.transform).GetComponent<Movement>().LevelMap = this.gameObject;
        
        CreateLevel();
        
        Queue<GameObject> obs = new Queue<GameObject>();

        obs = S_Pathfinder.PathFind(tiles, new Vector2(3, 0), new Vector2(2, 9));

        while(obs.Count > 0)
        {
            GameObject ob = obs.Dequeue();
            ob.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
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

        mapSizeX = mapXSize;
        mapSizeY = mapYSize;

        tiles = new GameObject[mapXSize, mapYSize];

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


        // hacky level wall loops
        for(int x=-1; x < mapSizeX + 1; x++)
        {
            for (int y = -1; y <= mapSizeY + 1; y += mapSizeY+1)
            {
                Instantiate(barrier, new Vector3(x + .5f, y + .5f, 1.0f), Quaternion.identity);
            }
        }
        for(int y = 0; y < mapYSize; y++)
        {
            for(int x = -1; x < mapSizeX + 1; x += mapSizeX + 1)
            {
                Instantiate(barrier, new Vector3(x + .5f, (mapSizeY - 1) - y + .5f, 1.0f), Quaternion.identity);
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
        tiles[x, y] = Instantiate(tilePrefabs[tileIndex], new Vector3(x + 0.5f, (yStart - y) + 0.5f, 1.0f), Quaternion.identity);
        tiles[x, y].transform.parent = transform;

        Tile tileScript = tiles[x, y].GetComponent<Tile>();
        tileScript.X = x;
        tileScript.Y = y;

        if (tileIndex == 1)
            tileScript.IsWalkable = true;
        else
            tileScript.IsWalkable = false;

        // Move current tile object by the proper 2D array space
        //newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
    }

    private string[] ReadLevelText()
    {
        // Currently loading level-1 as a default. Will need to be changed to allow for multiple loading of files given level progression.

        // Bind the txt file as data as a text asset
        TextAsset bindData = Resources.Load(level) as TextAsset;

        print(bindData);

        // Parse the string of tileIntegers from the text asset
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        // Return the entire string array, splitting indexes by the '-' mark [ mark signifies new row of tiles ]
        return data.Split('-');
    }
}
