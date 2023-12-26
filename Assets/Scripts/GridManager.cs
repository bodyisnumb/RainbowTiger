using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject rectanglePrefab; // Reference to your rectangle-shaped prefab
    public int rows = 5; // Number of rows in the grid
    public int columns = 5; // Number of columns in the grid
    public float spacing = 2.0f; // Spacing between each cell

    private Dictionary<Vector2Int, GameObject> gridCells; // Dictionary to store grid cells
    private Vector2 cellCenterOffset; // Offset for placing objects at cell centers

    // Offset values for the grid position
    private float gridOffsetX = -1.5f; // Adjust these values as needed
    private float gridOffsetY = -3.5f; // Adjust these values as needed

    void Start()
    {
        GenerateGrid();
        CalculateCellCenterOffset();
    }

    void GenerateGrid()
    {
        gridCells = new Dictionary<Vector2Int, GameObject>(); // Initialize the dictionary

        Vector3 spawnPosition = transform.position; // Starting position at the GameObject's position
        Vector3 originalSize = rectanglePrefab.GetComponent<Renderer>().bounds.size;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                spawnPosition = transform.position +
                    new Vector3(col * (originalSize.x + spacing), row * (originalSize.y + spacing), 0);
                GameObject newRectangle = Instantiate(rectanglePrefab, spawnPosition, Quaternion.identity);
                newRectangle.transform.parent = transform; // Organize under GridManager

                Vector2Int gridPos = new Vector2Int(col, row);
                gridCells.Add(gridPos, newRectangle); // Add the cell to the dictionary
            }
        }
    }

    public Dictionary<Vector2Int, GameObject> GetGrid()
    {
        return gridCells; // Return the dictionary containing grid cells
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        Vector2 cellSize = new Vector2(rectanglePrefab.transform.localScale.x + spacing, rectanglePrefab.transform.localScale.y + spacing);
        return new Vector2(x * cellSize.x + cellCenterOffset.x + gridOffsetX, y * cellSize.y + cellCenterOffset.y + gridOffsetY);
    }

    public Vector2Int GetGridPosition(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOffsetX) / (rectanglePrefab.transform.localScale.x + spacing));
        int y = Mathf.FloorToInt((worldPos.y - gridOffsetY) / (rectanglePrefab.transform.localScale.y + spacing));
        return new Vector2Int(x, y);
    }

    void CalculateCellCenterOffset()
    {
        // Calculate the center offset based on the size of the cell and spacing
        Vector2 cellSize = new Vector2(rectanglePrefab.transform.localScale.x + spacing, rectanglePrefab.transform.localScale.y + spacing);
        cellCenterOffset = cellSize / 2.0f;
    }

    public Vector2 GetCellCenterOffset()
    {
        return cellCenterOffset;
    }
}








