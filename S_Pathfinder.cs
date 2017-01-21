using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class S_Pathfinder
{
    //##################################################################
    //######################### V2 Pathfinder ##########################
    //##################################################################

    public static Queue<GameObject> PathFind(GameObject[,] Tiles, Vector2 start, Vector2 target)
    {
        PfNode[,] nodes = new PfNode[Tiles.GetLength(0), Tiles.GetLength(1)];

        int sX = (int)start.x;
        int sY = (int)start.y;

        int tX = (int)target.x;
        int tY = (int)target.y;

        for (int y = 0; y < nodes.GetLength(1); y++)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                Tile script = Tiles[x, y].GetComponent<Tile>();
                nodes[x, y] = new PfNode(x, y);
                nodes[x, y].IsWalkable = script.IsWalkable;
            }
        }

        List<PfNode> openList = new List<PfNode>();
        List<PfNode> closedList = new List<PfNode>();
        Queue<GameObject> pathQueue = new Queue<GameObject>();

        PfNode sNode = nodes[sX, sY];
        PfNode tNode = nodes[tX, tY];

        int F = Mathf.Abs(sX - tX) + Mathf.Abs(sY - tY);
        
        tNode.Hcost = F;
        tNode.Gcost = 0;
        tNode.Fcost = tNode.Gcost + tNode.Hcost;

        openList.Add(tNode);
        
        while (true)
        {
            PfNode current = SearchForNext(ref openList);

            if (current == null)
                break;

            closedList.Add(current);
            openList.Remove(current);

            List<PfNode> currNeighbours = GetNeighbours(current, nodes);

            ManageNeighbours(current, currNeighbours, openList, closedList, sNode);

            if (Equals(current, sNode))
                break;
        }
        
        PfNode currNode = sNode;

        while (currNode.Parent != null)
        {
            currNode = currNode.Parent;
            pathQueue.Enqueue(Tiles[currNode.X, currNode.Y]);
        }

        return pathQueue;
    }

    private static List<PfNode> GetNeighbours(PfNode current, PfNode[,] nodes)
    {
        List<PfNode> neighbours = new List<PfNode>();

        int maxX = nodes.GetLength(0);
        int maxY = nodes.GetLength(1);

        if (current.X + 1 < maxX)
            neighbours.Add(nodes[current.X + 1, current.Y]);

        if (current.X - 1 >= 0)
            neighbours.Add(nodes[current.X - 1, current.Y]);

        if (current.Y + 1 < maxY)
            neighbours.Add(nodes[current.X, current.Y + 1]);

        if (current.Y - 1 >= 0)
            neighbours.Add(nodes[current.X, current.Y - 1]);

        return neighbours;
    }

    private static PfNode SearchForNext(ref List<PfNode> openList)
    {
        int lowestF = int.MaxValue;

        PfNode curr = null;

        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].Fcost < lowestF)
            {
                curr = openList[i];
                lowestF = openList[i].Fcost;
            }
        }

        return curr;
    }

    private static void ManageNeighbours(PfNode current, List<PfNode> neighbours, List<PfNode> openList, List<PfNode> closedList, PfNode sNode)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (CheckNeighbourValidity(neighbours[i], closedList))
                continue;

            HandleValidNeighbour(current, neighbours[i], openList, sNode);
        }
    }

    private static bool CheckNeighbourValidity(PfNode curr, List<PfNode> closedList)
    {
        return (closedList.Contains(curr) || !curr.IsWalkable);
    }

    private static void HandleValidNeighbour(PfNode current, PfNode curr, List<PfNode> openList, PfNode start)
    {
        if (!openList.Contains(curr))
        {
            int H = Mathf.Abs(start.X - current.X) + Mathf.Abs(start.Y - current.Y);
            
            curr.Gcost = current.Gcost + 1;
            curr.Hcost = H;
            curr.Fcost = curr.Gcost + curr.Hcost;

            curr.Parent = current;

            openList.Add(curr);
            return;
        }
        else
        {
            int currentG = current.Gcost;

            int currentF = current.Fcost;

            int predictG = currentG + 1;
            int predictF = predictG + curr.Hcost;

            if (predictF < curr.Fcost)
            {
                curr.Gcost = predictG;
                curr.Fcost = predictF;

                curr.Parent = current;
            }
        }
    }
}
