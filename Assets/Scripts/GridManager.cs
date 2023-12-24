using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject rectanglePrefab; // Reference to your rectangle-shaped prefab
    public int rows = 5; // Number of rows in the grid
    public int columns = 5; // Number of columns in the grid
    public float spacing = 2.0f; // Spacing between each cell

    private Dictionary<Vector2Int, GameObject> gridCells; // Dictionary to store grid cells

    void Start()
    {
        GenerateGrid();
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
                    new Vector3(col * (originalSize.x + spacing), row * originalSize.y, 0);
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
}





