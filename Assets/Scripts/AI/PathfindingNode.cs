using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private Vector2Int position;
    public int stepsToPathtaker = int.MaxValue;
    public float airDistanceToDestination;
    public PathfindingNode predecessor = null;

    public Vector2Int Position { get => position; set => position = value; }

    public PathfindingNode(Vector2Int position)
    {
        this.position = position;
    }

    public PathfindingNode(Vector2Int position, int stepsToPathtaker, Vector2Int destination, PathfindingNode predecessor) : this(position)
    {
        this.stepsToPathtaker = stepsToPathtaker;
        this.airDistanceToDestination = (destination - position).magnitude;
        this.predecessor = predecessor;
    }
    /// <summary>
    /// returns each node leading towards this PathfindingNode
    /// </summary>
    /// <returns></returns>
    public List<PathfindingNode> EachStepToNode()
    {
        List<PathfindingNode> allNodes = new List<PathfindingNode>();

        PathfindingNode current = this;

        if (current.predecessor == null)
        {
            allNodes.Add(this);
            return allNodes;
        }

        while (current.predecessor != null)
        {
            allNodes.Add(current);
            current = current.predecessor;
        }
        allNodes.Add(current);

        List<PathfindingNode> rightOrder = new List<PathfindingNode>();

        for (int i = allNodes.Count-1; i >= 0; i--)
        {
            rightOrder.Add(allNodes[i]);
        }

        return rightOrder;
    }
}
