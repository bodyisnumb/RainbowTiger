using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GridManager gridManager; // Reference to your GridManager script
    public GameObject colorCrystalPrefab; // Reference to the color crystal prefab
    private Material[] colorMaterials; // Array of materials for different colors

    private Dictionary<Vector2Int, GameObject> bricks;
    private Dictionary<Vector2Int, Color> brickColors;
    private Dictionary<Vector2Int, GameObject> colorCrystals;

    private int currentLevel = 100;
    private int colorsAvailableAtStart = 3;
    private int colorsToAddEveryFiveLevels = 1;

    private List<Color> rainbowColors = new List<Color> {
        Color.red,
        new Color(1.0f, 0.5f, 0.0f), // Orange
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.5f, 0.0f, 0.5f) // Purple
    };

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        InitializeLevel();
        GenerateColorBrickStructure();
        RegenerateCrystals();
    }

    void InitializeLevel()
    {
        // Initialize available colors based on the current level and constraints
        List<Color> availableColors = new List<Color>();

        int colorsCount = Mathf.Min(colorsAvailableAtStart + (currentLevel - 1) / 5, rainbowColors.Count);
        for (int i = 0; i < colorsCount; i++)
        {
            availableColors.Add(rainbowColors[i]);
        }

        // Get the grid structure from GridManager
        bricks = gridManager.GetGrid();
        brickColors = new Dictionary<Vector2Int, Color>();
        colorCrystals = new Dictionary<Vector2Int, GameObject>();

        // Set initial color for each brick
        foreach (var brick in bricks)
        {
            Vector2Int position = brick.Key;
            Color randomColor = availableColors[Random.Range(0, availableColors.Count)];
            brickColors[position] = randomColor;
            brick.Value.GetComponent<Renderer>().material.color = randomColor;
        }
    }

    void GenerateColorBrickStructure()
    {
        foreach (var brick in bricks)
        {
            Vector2Int position = brick.Key;
            Color currentColor = brickColors[position];
            
            bool hasNeighborOfSameColor = HasNeighborOfSameColor(position, currentColor);
            if (!hasNeighborOfSameColor)
            {
                Vector2Int? bestNeighborPosition = GetBestNeighborWithColor(position);
                if (bestNeighborPosition != null)
                {
                    Vector2Int neighborPos = (Vector2Int)bestNeighborPosition;
                    brickColors[position] = brickColors[neighborPos];
                    bricks[position].GetComponent<Renderer>().material.color = brickColors[position];
                }
                // You can add further logic here to handle cases when there's no adjacent brick of the same color
            }
        }
        RegenerateCrystals();
    }

    Vector2Int? GetBestNeighborWithColor(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int? bestNeighbor = null;
        int maxAdjacentCount = 0;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (brickColors.ContainsKey(neighborPos))
            {
                int adjacentCount = GetAdjacentColoredBricksCount(neighborPos);
                if (adjacentCount > maxAdjacentCount)
                {
                    maxAdjacentCount = adjacentCount;
                    bestNeighbor = neighborPos;
                }
            }
        }
        return bestNeighbor;
    }

    int GetAdjacentColoredBricksCount(Vector2Int position)
    {
        int count = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (brickColors.ContainsKey(neighborPos))
            {
                count++;
            }
        }
        return count;
    }

    bool HasNeighborOfSameColor(Vector2Int position, Color currentColor)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (brickColors.ContainsKey(neighborPos) && brickColors[neighborPos] == currentColor)
            {
                return true;
            }
        }
        return false;
    }

    List<Color> GetAccessibleColors(Vector2Int position)
    {
        List<Color> accessibleColors = new List<Color>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (brickColors.ContainsKey(neighborPos))
            {
                Color neighborColor = brickColors[neighborPos];
                if (!accessibleColors.Contains(neighborColor))
                {
                    accessibleColors.Add(neighborColor);
                }
            }
        }
        return accessibleColors;
    }

    void PlaceCrystalOnBrick(GameObject brick, Color color)
    {
        Vector2Int brickPosition = new Vector2Int((int)brick.transform.position.x, (int)brick.transform.position.y);
        Vector3 crystalPosition = brick.transform.position;
        crystalPosition.z = -1; // Set the Z position

        GameObject crystal = Instantiate(colorCrystalPrefab, crystalPosition, Quaternion.identity);
        crystal.GetComponent<SpriteRenderer>().color = color;
        colorCrystals[brickPosition] = crystal;
    }

    void RegenerateCrystals()
    {
        HashSet<Color> placedColors = new HashSet<Color>(); // Track colors already assigned to crystals

        Vector2Int startingPosition = new Vector2Int(0, 0); // Replace this with your actual starting position
        Color startingColor = brickColors[startingPosition]; // Color of the starting cell

        foreach (var brick in bricks)
        {
            Vector2Int position = brick.Key;
            Color currentColor = brickColors[position];
            List<Color> accessibleColors = GetAccessibleColors(position);

            if (!colorCrystals.ContainsKey(position) && accessibleColors.Contains(currentColor) && position != startingPosition)
            {
                if (!placedColors.Contains(currentColor))
                {
                    Color chosenColor = GetDifferentColor(currentColor, accessibleColors);
                    if (chosenColor != Color.clear)
                    {
                        PlaceCrystalOnBrick(bricks[position], chosenColor); // Place a crystal with a different color
                        placedColors.Add(chosenColor); // Add the color to the set of placed colors
                    }
                }
            }
        }
    }

    Color GetDifferentColor(Color currentColor, List<Color> accessibleColors)
    {
        List<Color> colorsWithoutCurrent = new List<Color>(accessibleColors);
        colorsWithoutCurrent.Remove(currentColor);

        foreach (var color in colorsWithoutCurrent)
        {
            if (color != currentColor)
            {
                return color;
            }
        }

        return Color.clear; // Return a clear color if no different color is found
    }
}







