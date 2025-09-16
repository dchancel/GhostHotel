using System.Collections.Generic;
using UnityEngine;

public static class GridManager
{
    public static Dictionary<Vector2,GameObject> grid = new Dictionary<Vector2,GameObject>();

    private static int scaleFactor = 5;
    private static Vector2 gridSize = new Vector2(10,10);

    private static void SaveGrid()
    {

    }

    private static void LoadGrid()
    {

    }

    private static void ClearGrid()
    {
        grid.Clear();
    }

    private static bool GridIsFree(Vector2 location)
    {
        if (grid.ContainsKey(location))
        {
            if (grid[location] != null)
            {
                return false;
            }
        }
        return true;
    }

    private static void AddToGrid(Vector2 location, GameObject prefab)
    {
        if (!GridIsFree(location))
        {
            Debug.LogError("Tried to add " + prefab.name + " to grid location (" + location + ") while grid location is already in use by " + grid[location].name);
            return;
        }

        if (grid.ContainsKey(location))
        {
            RemoveFromGrid(location);
        }

        grid.Add(location, prefab);
    }

    private static void RemoveFromGrid(Vector2 location)
    {
        grid[location] = null;
        grid.Remove(location);
    }
}
