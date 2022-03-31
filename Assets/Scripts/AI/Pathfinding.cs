using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Pathfinding algorithm is based on A*
public static class Pathfinding 
{

    /// <summary>
    /// find a path from startPosition to destination and return last node in that path
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static PathfindingNode FindPath(Vector2Int startPosition, Vector2Int destination)
    {
        if (MapContent.instance == null || IsoGrid.instance == null)
        {
            return null;
        }
        //path is destination
        if (startPosition == destination)
        {
            return new PathfindingNode(startPosition, 0, destination, null);
        }
        //keep track of all searched tiles
        List<PathfindingNode> open = new List<PathfindingNode>();
        List<PathfindingNode> closed = new List<PathfindingNode>();

        open.Add(new PathfindingNode(startPosition, 0, destination, null));
        //as long as no path has been found and there are still spaces to search
        while (open.Count != 0)
        {
            //see which node has the lowest cost and select it as current bestNode
            PathfindingNode bestNode=null;
            foreach (PathfindingNode current in open)
            {
                if (bestNode == null)
                {
                    bestNode = current;
                }
                else
                {
                    if(bestNode.stepsToPathtaker + bestNode.airDistanceToDestination > current.stepsToPathtaker + current.airDistanceToDestination)
                    {
                        bestNode = current;
                    }
                }
            }

            //check each adjacent space if path to destination was found, return it

            Vector2Int neighbourPosition = new Vector2Int(bestNode.Position.x + 1, bestNode.Position.y);
            PathfindingNode pathToDestination = CheckNode(bestNode, open, closed, destination, neighbourPosition);
            if (pathToDestination != null)
                return pathToDestination;
            
            neighbourPosition = new Vector2Int(bestNode.Position.x - 1, bestNode.Position.y);
            pathToDestination = CheckNode(bestNode, open, closed, destination, neighbourPosition);
            if (pathToDestination != null)
                return pathToDestination;

            neighbourPosition = new Vector2Int(bestNode.Position.x, bestNode.Position.y + 1);
            pathToDestination = CheckNode(bestNode, open, closed, destination, neighbourPosition);
            if (pathToDestination != null)
                return pathToDestination;

            neighbourPosition = new Vector2Int(bestNode.Position.x, bestNode.Position.y - 1);
            pathToDestination = CheckNode(bestNode, open, closed, destination, neighbourPosition);
            if (pathToDestination != null)
                return pathToDestination;

            //remove bestNode from open list and add to closed List
            closed.Add(bestNode);
            open.Remove(bestNode);
        }
        return null;
    }
    /// <summary>
    /// perform all checks on a given tile, add it to the open list
    /// if path to destination is found, return it
    /// </summary>
    /// <param name="bestNode"></param>
    /// <param name="openList"></param>
    /// <param name="closedList"></param>
    /// <param name="destination"></param>
    /// <param name="positionToCheck"></param>
    /// <returns></returns>
    private static PathfindingNode CheckNode(PathfindingNode bestNode, List<PathfindingNode> openList, List<PathfindingNode> closedList, Vector2Int destination, Vector2Int positionToCheck)
    {
        //tiles with units blocking them are skipped
        if (MapContent.instance.Dictionary.ContainsKey(positionToCheck))
        {
            return null;
        }
        //check if there is a know node whith this position
        foreach (PathfindingNode oldNodes in openList)
        {
            if (oldNodes.Position == positionToCheck)
            {
                //check if known position could be reached through a shorter path from current bestNodes direction
                if (oldNodes.stepsToPathtaker > bestNode.stepsToPathtaker + 1)
                {
                    //new shorter path to reach oldNode has been found, change its predecessor
                    oldNodes.stepsToPathtaker = bestNode.stepsToPathtaker + 1;
                    oldNodes.predecessor = bestNode;
                }
                return null;
            }
        }
        //check if there is a know node whith this position
        foreach (PathfindingNode oldNodes in closedList)
        {
            if (oldNodes.Position == positionToCheck)
            {
                //check if known position could be reached through a shorter path from current bestNodes direction
                if (oldNodes.stepsToPathtaker > bestNode.stepsToPathtaker + 1)
                {
                    //new shorter path to reach oldNode has been found, change its predecessor
                    oldNodes.stepsToPathtaker = bestNode.stepsToPathtaker + 1;
                    oldNodes.predecessor = bestNode;
                }
                return null;
            }
        }

        if (IsoGrid.instance.IsInsideBounds(positionToCheck))
        {
            //if this position is the destination, return the pathfinding node containing the destination
            if (positionToCheck == destination)
            {
                return new PathfindingNode(positionToCheck, bestNode.stepsToPathtaker + 1, destination, bestNode);
            }
            //add new position to openList for further use
            openList.Add(new PathfindingNode(positionToCheck, bestNode.stepsToPathtaker + 1, destination, bestNode));
        }    
        return null;
    }
}
