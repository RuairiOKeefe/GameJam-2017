using UnityEngine;
using System.Collections;

public class PfNode
{
    //A* costs
    public int Fcost { get; set; }
    public int Gcost { get; set; }
    public int Hcost { get; set; }

    // Node Coords
    public int X { get; set; }
    public int Y { get; set; }

    // Node Walkableness
    public bool IsWalkable { get; set; }

    // ParentNode
    public PfNode Parent { get; set; }

    public PfNode(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void UpdateCosts(int hX, int hY, int gX, int gY)
    {
        Hcost = hX + hY;
        Gcost = gX + gY;

        Fcost = Hcost + Gcost;
    }
}
