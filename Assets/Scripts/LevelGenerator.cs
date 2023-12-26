using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject colorCrystalPrefab;
    public GameObject progressCrystalPrefab;
    public GameObject tigerPrefab;
    private Material[] colorMaterials;

    private Dictionary<Vector2Int, GameObject> bricks;
    private Dictionary<Vector2Int, Color> brickColors;
    private Dictionary<Vector2Int, GameObject> colorCrystals;
    private Dictionary<Vector2Int, GameObject> progressCrystals;
    

    private int currentLevel = 1;
    private int colorsAvailableAtStart = 3;
//    private int colorsToAddEveryFiveLevels = 1;

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
        SpawnTigerOnFirstCell();
        GenerateLevel();
        
    }

    void GenerateLevel()
    {
        InitializeLevel();
        GenerateColorBrickStructure();
        
        GenerateProgressCrystals();
    }

    void InitializeLevel()
    {
        List<Color> availableColors = new List<Color>();

        int colorsCount = Mathf.Min(colorsAvailableAtStart + (currentLevel - 1) / 5, rainbowColors.Count);
        for (int i = 0; i < colorsCount; i++)
        {
            availableColors.Add(rainbowColors[i]);
        }

        bricks = gridManager.GetGrid();
        brickColors = new Dictionary<Vector2Int, Color>();
        colorCrystals = new Dictionary<Vector2Int, GameObject>();
        progressCrystals = new Dictionary<Vector2Int, GameObject>();

        foreach (var brick in bricks)
        {
            Vector2Int position = brick.Key;
            Color randomColor = availableColors[Random.Range(0, availableColors.Count)];
            brickColors[position] = randomColor;
            brick.Value.GetComponent<Renderer>().material.color = randomColor;
        }
        RegenerateCrystals();
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
        HashSet<Color> placedColors = new HashSet<Color>();

        Vector2Int startingPosition = new Vector2Int(0, 0);

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
                        PlaceCrystalOnBrick(bricks[position], chosenColor);
                        placedColors.Add(chosenColor);
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

void GenerateProgressCrystals()
{
    List<Vector2Int> availablePositions = new List<Vector2Int>(bricks.Keys);
    Vector2Int startingPosition = new Vector2Int(0, 0);
    availablePositions.Remove(startingPosition);

    // Get positions adjacent to color crystals
    HashSet<Vector2Int> positionsToRemove = new HashSet<Vector2Int>();
    foreach (var crystal in colorCrystals)
    {
        Vector2Int crystalPosition = crystal.Key;
        RemoveNearbyPositions(crystalPosition, availablePositions, positionsToRemove);
    }

    // Remove positions already occupied by progress crystals
    foreach (var crystal in progressCrystals)
    {
        availablePositions.Remove(crystal.Key);
    }

    // Remove positions adjacent to color crystals
    foreach (var pos in positionsToRemove)
    {
        availablePositions.Remove(pos);
    }

    int numberOfProgressCrystals = Mathf.Min(availablePositions.Count / 4, 10);

    for (int i = 0; i < numberOfProgressCrystals; i++)
    {
        Vector2Int randomPosition = availablePositions[Random.Range(0, availablePositions.Count)];

        // Check if the random position is not occupied by a progress crystal
        if (!progressCrystals.ContainsKey(randomPosition))
        {
            PlaceProgressCrystalOnBrick(bricks[randomPosition]);
            progressCrystals[randomPosition] = progressCrystalPrefab;

            // Remove nearby positions to prevent spawning on adjacent tiles
            RemoveNearbyPositions(randomPosition, availablePositions);
        }
        else
        {
            i--; // Retry placing progress crystal at another position
        }

        availablePositions.Remove(randomPosition);
    }
}

void RemoveNearbyPositions(Vector2Int position, List<Vector2Int> availablePositions, HashSet<Vector2Int> positionsToRemove = null)
{
    if (positionsToRemove == null)
    {
        positionsToRemove = new HashSet<Vector2Int>();
    }

    // Define positions that are adjacent to the given position
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    foreach (Vector2Int dir in directions)
    {
        Vector2Int adjacentPos = position + dir;
        if (availablePositions.Contains(adjacentPos))
        {
            positionsToRemove.Add(adjacentPos);
        }
    }

    // Now remove the nearby positions from available positions
    foreach (var pos in positionsToRemove)
    {
        availablePositions.Remove(pos);
    }
}

    void PlaceProgressCrystalOnBrick(GameObject brick)
    {
        Vector2Int brickPosition = new Vector2Int((int)brick.transform.position.x, (int)brick.transform.position.y);
        Vector3 crystalPosition = brick.transform.position;
        crystalPosition.z = -1;

        GameObject crystal = Instantiate(progressCrystalPrefab, crystalPosition, Quaternion.identity);
        progressCrystals[brickPosition] = crystal;

        // Check if there's a color crystal on the same brick position
        if (colorCrystals.ContainsKey(brickPosition))
        {
            Destroy(colorCrystals[brickPosition]); // Remove the color crystal
            colorCrystals.Remove(brickPosition); // Remove it from the dictionary
        }
    }

    void SpawnTigerOnFirstCell()
    {
        // Get the position of the first cell in the grid
        Vector2Int firstCellPosition = new Vector2Int(0, 0);
        Vector2 cellCenter = gridManager.GetWorldPosition(firstCellPosition.x, firstCellPosition.y) +
                            gridManager.GetCellCenterOffset(); // Assuming you have a method to get the center offset

        // Instantiate the Tiger at the center of the first cell
        GameObject tiger = Instantiate(tigerPrefab, cellCenter, Quaternion.identity);
    }

}








