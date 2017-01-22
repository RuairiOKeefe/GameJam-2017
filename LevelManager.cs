using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] tilePrefabs;

    // Stores all map tiles
    public GameObject[,] tiles;

    public GameObject mover;
    public List<GameObject> EnemyPrefabs;
    public Transform start;
    public Vector2 EnemyObjectiveCoords;
    public List<GameObject> Turrets;

    public GameObject castlePrefab;
    public GameObject castle;

    public GameObject stateManager;

    // Stores map dimesnions
    public int mapSizeX;
    public int mapSizeY;


    public int CurrLevel;
    public int LevelWaves;
    public int CurrWave;
    // Array used to balance out the spawn load across various start lanes.
    public List<int> LaneSpawnCounts;
    public List<GameObject> CandidateTiles;
    public List<GameObject> Enemies;
    // Used to determine the hardest unit that can spawn
    public int MaxSpawnIndex;
    // Array used to skew the chances of harder units to spawn so the player doesn't get RNG screwed.
    public int[] SpawnTypeChanceDistribution;

    // Timing variables for time between waves
    public int WavesToSpawn;
    public int MaxWaves;
    public float InterWaveTime;
    public float TimeToWave;
    public bool IsSpawningWave;

    // Time variables for spawning individual enemies
    public float InterEnemeySpawn;
    public float CurrSpawnTime;

    // Number of enemies to spawn
    public int NumEnemiesToSpawn;

    // Variables to check state of success
    public bool IsGameOver;
    public bool IsGameComplete;
    public bool IsLevelComplete;

    public float TileSize
    {
        // Getter for tileSize
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Awake()
    {
        Enemies = new List<GameObject>();
    }

	// Use this for initialization
	void Start ()
    {
        CurrLevel = 1;
        CreateLevel(CurrLevel);
	}
	
	// Update is called once per frame
	void Update ()
    {
        WaveHandler();

        if (IsSpawningWave)
        {
            SpawnWave();
        }

        if (IsGameOver)
        {
            stateManager.GetComponent<StateManager>().ChangeState(0);
            ResetGame();
        }

        if (WavesToSpawn <= 0 && Enemies.Count <= 0)
        {
            if (CurrLevel == 5)
            {
                IsGameComplete = true;
                stateManager.GetComponent<StateManager>().ChangeState(0);
                ResetGame();
            }

            CurrLevel++;
            DestroyLevel();
            CreateLevel(CurrLevel);
        }
	}

    private void ResetGame()
    {
        TimeToWave = 30;
        CurrLevel = 1;
        DestroyLevel();
    }

    private void CreateLevel(int level)
    {
        // Load in map data as a string array - see method for more info
        string[] mapData = ReadLevelText(level);

        // Sets the map X and Y sizes
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        mapSizeX = mapXSize;
        mapSizeY = mapYSize;

        tiles = new GameObject[mapXSize, mapYSize];

        MaxWaves = 5 * level;
        WavesToSpawn = MaxWaves;

        TimeToWave = InterWaveTime;
        CurrSpawnTime = InterEnemeySpawn;
        

        // Initialized the world start set to the camera position

        // TODO - need to initialize based on a fixed camera size for level 1 which doubles the X and Y ranges (zooms out) for levels 2+
        print(mapYSize);
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, mapYSize));

        LaneSpawnCounts = new List<int>();
        CandidateTiles = new List<GameObject>();

        

        int Spawny = 0;

        for(int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, mapYSize-1);
            }
        }

        switch (level)
        {
            case 1:
                LaneSpawnCounts.Add(0);
                CandidateTiles.Add(tiles[1, 1]);
                EnemyObjectiveCoords = new Vector2(5, 10);
                break;
            case 2:
                LaneSpawnCounts.Add(0);
                CandidateTiles.Add(tiles[3, 0]);
                EnemyObjectiveCoords = new Vector2(2, 9);
                break;
            case 3:
                LaneSpawnCounts.Add(0);
                LaneSpawnCounts.Add(0);
                CandidateTiles.Add(tiles[3, 2]);
                CandidateTiles.Add(tiles[10, 2]);
                EnemyObjectiveCoords = new Vector2(5, 20);
                break;
            case 4:
                LaneSpawnCounts.Add(0);
                LaneSpawnCounts.Add(0);
                CandidateTiles.Add(tiles[4, 1]);
                CandidateTiles.Add(tiles[9, 3]);
                EnemyObjectiveCoords = new Vector2(6, 20);
                break;
            case 5:
                LaneSpawnCounts.Add(0);
                LaneSpawnCounts.Add(0);
                LaneSpawnCounts.Add(0);
                CandidateTiles.Add(tiles[0, 2]);
                CandidateTiles.Add(tiles[6, 2]);
                CandidateTiles.Add(tiles[12, 2]);
                EnemyObjectiveCoords = new Vector2(6, 20);
                break;
        }

        
        castle = Instantiate(castlePrefab, tiles[(int)EnemyObjectiveCoords.x, (int)EnemyObjectiveCoords.y].transform.position, Quaternion.identity);
        castle.GetComponent<Health>().LevelMap = this.gameObject;

        //bool hasBroken = false;
        //for (int y = 0; y < tiles.GetLength(1) && !hasBroken; y++)
        //{
        //    for (int x = 0; x < tiles.GetLength(0); x++)
        //    {
        //        Tile script = tiles[x, y].GetComponent<Tile>();

        //        if (script.IsWalkable)
        //        {
        //            Spawny = y;
        //            hasBroken = true;
        //            break;
        //        }
        //    }
        //}

        //for (int x = 0; x < tiles.GetLength(0); x++)
        //{
        //    Tile script = tiles[x, Spawny].GetComponent<Tile>();

        //    if (script.IsWalkable)
        //    {
        //        LaneSpawnCounts.Add(0);
        //        CandidateTiles.Add(tiles[x, Spawny]);
        //    }
        //}

        MaxSpawnIndex = 7 / (7 - (CurrLevel + 1));
    }

    private void PlaceTile(string tileType, int x, int y, float yStart)
    {
        // Places a tile based on the prefab index of the Level Manager. Index is parsed as an Integer from the text file [0 for beach, 1 for trench, etc]

        // Get next tile index
        int tileIndex = int.Parse(tileType);

        // Instantiate new tile object
        //GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        GameObject currTile = Instantiate(tilePrefabs[tileIndex], new Vector3(x + 0.5f, (yStart - y) + 0.5f, 1.0f), Quaternion.identity);
        tiles[x, y] = currTile;
        tiles[x, y].transform.parent = transform;

        Tile tileScript = tiles[x, y].GetComponent<Tile>();
        tileScript.X = x;
        tileScript.Y = y;

        if (tileIndex == 1 || tileIndex == 6)
        {
            tileScript.IsWalkable = true;
        }
        else
        {
            tileScript.IsWalkable = false;
        }

        // Move current tile object by the proper 2D array space
        //newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
    }

    private string[] ReadLevelText(int level)
    {
        // Currently loading level-1 as a default. Will need to be changed to allow for multiple loading of files given level progression.

        // Bind the txt file as data as a text asset
        TextAsset bindData = Resources.Load("Level-" + level) as TextAsset;

        print(bindData);

        // Parse the string of tileIntegers from the text asset
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        // Return the entire string array, splitting indexes by the '-' mark [ mark signifies new row of tiles ]
        return data.Split('-');
    }

    private void DestroyLevel()
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                Destroy(tiles[x, y]);
            }
        }

        Destroy(castle);
        castle = null;

        for (int i = 0; i < Turrets.Count; i++)
        {
            Destroy(Turrets[i]);
        }
        Turrets = new List<GameObject>();

        for (int i = 0; i < Enemies.Count; i++)
        {
            Destroy(Enemies[i]);
        }
        Enemies = new List<GameObject>();

        tiles = new GameObject[1, 1];
    }

    private void WaveHandler()
    {
        if (WavesToSpawn > 0)
        {
            if (TimeToWave <= 0)
            {
                NumEnemiesToSpawn = ((MaxWaves - WavesToSpawn + 1) * 3) + (2 * CurrLevel);
            }

            if (TimeToWave <= 0 || IsSpawningWave)
            {
                TimeToWave = InterWaveTime;
                IsSpawningWave = true;
            }
            else
            {
                TimeToWave -= Time.deltaTime;
            }
        }
        else
        {
            IsLevelComplete = true;
        }
    }

    public void SpawnWave()
    {
        if (NumEnemiesToSpawn > 0)
        {
            if (CurrSpawnTime <= 0)
            {
                CurrSpawnTime = InterEnemeySpawn;
                NumEnemiesToSpawn--;

                SpawnEnemy();
            }
            else
            {
                CurrSpawnTime -= Time.deltaTime;
            }
        }
        else
        {
            IsSpawningWave = false;
            WavesToSpawn--;
        }
    }

    public void SpawnEnemy()
    {
        int LowestCount = int.MaxValue;
        int CandidateIndex = 0;

        for (int i = 0; i < LaneSpawnCounts.Count; i++)
        {
            if (LaneSpawnCounts[i] < LowestCount)
            {
                LowestCount = LaneSpawnCounts[i];
                CandidateIndex = i;
            }
            else
            {
                continue;
            }
        }

        int EnemeyPick = UnityEngine.Random.Range(0, 100);
        int[,] Chances = new int[2, MaxSpawnIndex];

        int currMax = 0;
        int prevMax = 0;

        for (int y = 0; y < Chances.GetLength(1); y++)
        {
            prevMax = currMax;
            currMax = (currMax + 100) / 2;

            Chances[0, y] = prevMax;
            Chances[1, y] = currMax;
        }

        int EnemyIndexToSpawn = 0;

        for (int i = 0; i < MaxSpawnIndex; i++)
        {
            if (EnemeyPick >= Chances[0, i] && EnemeyPick < Chances[1, i])
            {
                EnemyIndexToSpawn = i;
            }
            else
            {
                continue;
            }
        }

        EnemyIndexToSpawn = UnityEngine.Random.Range(0, MaxSpawnIndex);

        LaneSpawnCounts[CandidateIndex]++;
        //Tile script = CandidateTiles[CandidateIndex].GetComponent<Tile>();
        //Vector3 tilePos = new Vector3(script.X + 0.5f, script.Y + 0.5f, 0);
        GameObject enemy = Instantiate(EnemyPrefabs[EnemyIndexToSpawn], CandidateTiles[CandidateIndex].transform.position + Vector3.back, Quaternion.identity);
        Movement movescript = enemy.GetComponent<Movement>();
        Health HPscript = enemy.GetComponent<Health>();
        HPscript.enemyList = Enemies;
        movescript.LevelMap = this.gameObject;
        Enemies.Add(enemy);
    }
}
