using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Used to set circle position
    public Vector3 Position;
    // Used to detect if the next tile is close enough to be snapped to
    public Circle MovementCircle;
    // To determine rigidbody velocity
    public float Speed;
    // Rigidbody for movement
    public Rigidbody2D Rb;
    public Vector3 Vel;
    public Vector3 nextPos;

    // The X and Y Coordinates according to the map
    public int XCoord;
    public int YCoord;

    // Map dimension Y, for getting the Y coord due to the map being inverted on the Y axis
    public int MapSizeY;

    // The gameobject that contains the array of map tiles.
    public GameObject LevelMap;
    public LevelManager LevelManScript;

    // The queue of path tiles for the AI to follow.
    public Queue<GameObject> PathQueue;
    // Determine if character is moving
    public bool CanMove;

	// Use this for initialization
	void Start ()
    {
        Rb = GetComponent<Rigidbody2D>();

        
        transform.position = new Vector3(3.5f, 10.5f, 0);

        LevelManScript = LevelMap.GetComponent<LevelManager>();
        
        Position = transform.position;

        MovementCircle = new Circle(Position.x, Position.y, 0, 0.01f);

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetPosition();
        GetPath();

        if (CanMove)
        {
            PathMovement();
        }

        Position += Vel * Time.deltaTime;
        transform.position = Position;
	}

    public void GetPosition()
    {
        MapSizeY = LevelManScript.mapSizeY;

        XCoord = (int)Position.x;
        YCoord = (MapSizeY - 1) - (int)Position.y;
    }

    public void GetPath()
    {
        PathQueue = S_Pathfinder.PathFind(LevelManScript.tiles, new Vector2(XCoord, YCoord), new Vector2(2, 9));
    }

    public void PathMovement()
    {
        if (PathQueue.Count > 0)
        {
            if (MovementCircle.Contains(PathQueue.Peek().transform.position))
            {
                transform.position = PathQueue.Peek().transform.position;
                Position = transform.position;
                MovementCircle.Position = Position;

                PathQueue.Dequeue();

                Vel = Vector3.zero;
            }
            else
            {
                Vel = (PathQueue.Peek().transform.position - transform.position);
                Vel.Normalize();
                Vel *= Speed;

                Position = transform.position;
                MovementCircle.Position = Position;
            }
        }
        else
        {
            Vel = Vector3.zero;
        }
    }
}
