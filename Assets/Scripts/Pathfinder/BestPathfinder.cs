using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestPathfinder
{
    private GridManager Grid;
    private Tile StartPoint;
    private Tile EndPoint;
    private Vector2 EndPointPosition;
    private Tile CurrentTile;

    public BestPathfinder(GridManager grid, Vector2 startPointPosition, Vector2 endPointPosition)
    {
        Grid = grid;

        EndPointPosition = endPointPosition;
        EndPoint = Grid.GetTileAtPosition(endPointPosition);
        StartPoint = Grid.GetTileAtPosition(startPointPosition);
    }

    public List<TileLogic> GetBestPath()
    {
        List<TileLogic> from = new List<TileLogic>();
        List<TileLogic> openSet = new List<TileLogic>();
        openSet.Add(StartPoint.Logic);

        while (openSet.Count > 0)
        {
            int win = 0;

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].F > openSet[win].F)
                {
                    win = i;
                }

                if (openSet[i].F == openSet[win].F && openSet[i].G > openSet[win].G)
                {
                    win = i;
                }
            }

            TileLogic currentTile = openSet[win];
            currentTile.IsVisited = true;

            if (currentTile.Position == EndPointPosition)
            {
                return openSet;
            }

            openSet.RemoveAt(win);
            from.Add(currentTile);
            List<Tile> neighbors = Grid.GetNeighbors(currentTile.Position);
            List<TileLogic> logicNeighbors = toLogicNeighbors(currentTile.AccScore, neighbors);

            foreach (TileLogic neighborLogic in logicNeighbors)
            {

                if (!from.Contains(neighborLogic))
                {
                    int g = currentTile.G + CalculateNeighborHeuristic(currentTile, neighborLogic);

                    if (!openSet.Contains(neighborLogic))
                    {
                        openSet.Add(neighborLogic);
                    }
                    else if (g >= neighborLogic.G)
                    {
                        continue;
                    }

                    neighborLogic.G = g;
                    neighborLogic.H = CalculateHeuristic(neighborLogic, EndPoint.Logic);
                    neighborLogic.F = neighborLogic.G + neighborLogic.H;
                    neighborLogic.PrevTileLogic = currentTile;
                    neighborLogic.AccScore = currentTile.AccScore + neighborLogic.Score;
                }
            }
        }

        return null;
    }

    private List<TileLogic> toLogicNeighbors(int accScore, List<Tile> neighbors)
    {
        List<TileLogic> logicNeighbors = new List<TileLogic>();

        foreach (Tile neighbor in neighbors)
        {
            if (accScore + neighbor.Logic.Score > 0)
            {
                logicNeighbors.Add(neighbor.Logic);
            }
        }

        return logicNeighbors;
    }

    private int CalculateNeighborHeuristic(TileLogic from, TileLogic to)
    {
        int xShift = Math.Abs((int)(from.Position.x - to.Position.x));
        int yShift = Math.Abs((int)(from.Position.y - to.Position.y));

        return to.Score;
    }

    private int CalculateHeuristic(TileLogic from, TileLogic to)
    {
        int xShift = Math.Abs((int)(from.Position.x - to.Position.x));
        int yShift = Math.Abs((int)(from.Position.y - to.Position.y));

        return xShift + yShift;
    }
}
