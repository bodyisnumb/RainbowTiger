using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private LevelGenerator levelGenerator;
    private Vector2 direction;
    private bool isMoving;
    private Vector3 targetPosition;

    private int gridWidth;
    private int gridHeight;
    private bool gameStarted;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();

        gridWidth = levelGenerator.gridManager.columns;
        gridHeight = levelGenerator.gridManager.rows;

        // Set the starting position based on LevelGenerator
        Vector2Int initialCellPosition = new Vector2Int(-1, -1); // Adjust this to start within the grid

        // Get the center of the first cell specifically
        Vector2 cellCenter = levelGenerator.gridManager.GetWorldPosition(initialCellPosition.x, initialCellPosition.y) +
                             levelGenerator.gridManager.GetCellCenterOffset();

        targetPosition = new Vector3(cellCenter.x, cellCenter.y, -5f); // Set the initial Z position for visibility
        transform.position = targetPosition;
        direction = Vector2.up; // Start by moving up
        gameStarted = false;
    }

    void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            gameStarted = true;
        }

        if (gameStarted && !isMoving)
        {
            CheckInput(); // Check input for direction change
            MoveTiger();
        }
    }

    void MoveTiger()
    {
        Vector2 currentCell = levelGenerator.gridManager.GetGridPosition(transform.position);

        if (CanMoveInDirection(currentCell, direction))
        {
            Vector2Int currentCellInt = new Vector2Int((int)currentCell.x, (int)currentCell.y);

            Vector2Int nextCell = new Vector2Int(currentCellInt.x + (int)direction.x, currentCellInt.y + (int)direction.y); // Move in the current direction

            if (IsWithinGridBounds(nextCell))
            {
                Vector2 targetCellCenter = levelGenerator.gridManager.GetWorldPosition(nextCell.x, nextCell.y) +
                                           levelGenerator.gridManager.GetCellCenterOffset();

                targetPosition = new Vector3(targetCellCenter.x - 1, targetCellCenter.y - 1, -5f); // Set the target position to the center of the next cell
                isMoving = true;
            }
            else
            {
                ChangeDirection(currentCell); // Change direction upon reaching the edge
            }
        }
        else
        {
            ChangeDirection(currentCell); // Change direction upon reaching the edge
        }
    }

    void ChangeDirection(Vector2 currentCell)
    {
        Vector2Int currentCellInt = new Vector2Int((int)currentCell.x, (int)currentCell.y);

        // Reverse the direction to move in the opposite direction
        direction *= -1;

        Vector2Int nextCell = new Vector2Int(currentCellInt.x + (int)direction.x, currentCellInt.y + (int)direction.y);

        if (IsWithinGridBounds(nextCell))
        {
            Vector2 targetCellCenter = levelGenerator.gridManager.GetWorldPosition(nextCell.x, nextCell.y) +
                                       levelGenerator.gridManager.GetCellCenterOffset();

            targetPosition = new Vector3(targetCellCenter.x - 1, targetCellCenter.y - 1, -5f); // Set the target position to the center of the next cell
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    bool CanMoveInDirection(Vector2 cell, Vector2 dir)
    {
        // Check if the next cell in the current direction is within the grid bounds
        Vector2Int nextCell = new Vector2Int((int)(cell.x + dir.x), (int)(cell.y + dir.y));
        return IsWithinGridBounds(nextCell);
    }

    bool IsWithinGridBounds(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < gridWidth && cell.y >= 0 && cell.y < gridHeight;
    }

    void CheckInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector2(horizontal, vertical).normalized;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}






















