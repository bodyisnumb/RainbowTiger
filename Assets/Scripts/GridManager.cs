using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject rectanglePrefab;
    public int rows = 8;
    public int columns = 4;
    public float spacing = 2.0f;

    private Dictionary<Vector2Int, GameObject> gridCells;
    private Vector2 cellCenterOffset;

    private float gridOffsetX = -1.5f; 
    private float gridOffsetY = -3.5f; 

    void Start()
    {
        GenerateGrid();
        CalculateCellCenterOffset();
    }

    void GenerateGrid()
    {
        gridCells = new Dictionary<Vector2Int, GameObject>();

        Vector3 spawnPosition = transform.position;
        Vector3 originalSize = rectanglePrefab.GetComponent<Renderer>().bounds.size;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                spawnPosition = transform.position +
                    new Vector3(col * (originalSize.x + spacing), row * (originalSize.y + spacing), 0);
                GameObject newRectangle = Instantiate(rectanglePrefab, spawnPosition, Quaternion.identity);
                newRectangle.transform.parent = transform;

                Vector2Int gridPos = new Vector2Int(col, row);
                gridCells.Add(gridPos, newRectangle);
            }
        }
    }

    public Dictionary<Vector2Int, GameObject> GetGrid()
    {
        return gridCells;
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
        Vector2 cellSize = new Vector2(rectanglePrefab.transform.localScale.x + spacing, rectanglePrefab.transform.localScale.y + spacing);
        cellCenterOffset = cellSize / 2.0f;
    }

    public Vector2 GetCellCenterOffset()
    {
        return cellCenterOffset;
    }

    public Color GetCellColor(Vector2Int cellPosition)
    {
        if (gridCells.TryGetValue(cellPosition, out GameObject cellObject))
        {
            SpriteRenderer cellRenderer = cellObject.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                return cellRenderer.color;
            }
        }
        
        return Color.white;
    }

    public void UpdateCellColor(Vector2Int position, Color color)
    {
        if (gridCells.TryGetValue(position, out GameObject cellObject))
        {
            SpriteRenderer cellRenderer = cellObject.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                cellRenderer.color = color;
            }
        }
    }

    public void UpdateGridSize(int newColumns, int newRows)
    {
        columns = newColumns;
        rows = newRows;
        GenerateGrid();
    }
}








