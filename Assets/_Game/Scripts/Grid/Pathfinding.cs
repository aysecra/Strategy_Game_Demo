using System.Collections.Generic;
using System.Linq;

namespace PanteonDemo
{
    public static class Pathfinding
    {
        public static List<GridsCellBase> FindPath(GridsCellBase startCell, GridsCellBase targetCell)
        {
            List<GridsCellBase> toSearch = new List<GridsCellBase>(){startCell};
            List<GridsCellBase> processed = new List<GridsCellBase>();

            while (toSearch.Any())
            {
                GridsCellBase currentCell = toSearch[0];
                foreach (var searchCell in toSearch)
                {
                    if (searchCell.F <= currentCell.F && searchCell.H < currentCell.H)
                    {
                        currentCell = searchCell;
                    }
                }
                
                processed.Add(currentCell);
                toSearch.Remove(currentCell);

                if (currentCell == targetCell)
                {
                    GridsCellBase currentPathTile = targetCell;
                    List<GridsCellBase> path = new List<GridsCellBase>();
                    while (currentPathTile != startCell)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                    }

                    return path;
                }

                IEnumerable<GridsCellBase> walkableNeighbors =
                    currentCell.Neighbors.Where(cell => cell.Walkable && !processed.Contains((cell)));

                foreach (var neighbor in walkableNeighbors)
                {
                    bool inSearch = toSearch.Contains(neighbor);

                    float costToNeighbour = currentCell.G + currentCell.GetDistance(neighbor);

                    if (!inSearch || costToNeighbour < neighbor.G)
                    {
                        neighbor.G = costToNeighbour;
                        neighbor.Connection = currentCell;

                        if (!inSearch)
                        {
                            neighbor.H = neighbor.GetDistance(targetCell);
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
    }
}
