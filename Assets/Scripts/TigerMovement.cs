using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private LevelGenerator levelGenerator;
    private CrystalManager crystalManager;
    private GridManager gridManager;
    private Color currentCellColor;
    private Vector2 direction;
    private bool isMoving;
    private Vector3 targetPosition;

    private int gridWidth;
    private int gridHeight;
    private bool gameStarted;

    public GameObject spikes;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        crystalManager = FindObjectOfType<CrystalManager>();
        gridManager = FindObjectOfType<GridManager>();

        gridWidth = levelGenerator.gridManager.columns;
        gridHeight = levelGenerator.gridManager.rows;

        Vector2Int initialCellPosition = new Vector2Int(-1, -1);
        Vector2 cellCenter = levelGenerator.gridManager.GetWorldPosition(initialCellPosition.x, initialCellPosition.y) +
                             levelGenerator.gridManager.GetCellCenterOffset();

        targetPosition = new Vector3(cellCenter.x, cellCenter.y, -5f);
        transform.position = targetPosition;
        direction = Vector2.up;
        gameStarted = false;

        initialCellPosition = levelGenerator.gridManager.GetGridPosition(transform.position);
        Color initialCellColor = gridManager.GetCellColor(initialCellPosition);
        crystalManager.ChangeSafeColor(initialCellColor);

        spikes = transform.Find("Spikes").gameObject;
        ToggleSpikesBasedOnColor();
    }

    void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            gameStarted = true;
        }

        if (gameStarted && !isMoving)
        {
            CheckInput();
            MoveTiger();
        }

        UpdateCurrentCellColor();
        Debug.Log(currentCellColor);
    }

    void UpdateCurrentCellColor()
    {
        Vector2Int currentCellPosition = gridManager.GetGridPosition(transform.position);
        currentCellColor = gridManager.GetCellColor(currentCellPosition);
        
        ToggleSpikesBasedOnColor();
    }

    void MoveTiger()
    {
        Vector2 currentCell = levelGenerator.gridManager.GetGridPosition(transform.position);

        if (CanMoveInDirection(currentCell, direction))
        {
            Vector2Int currentCellInt = new Vector2Int((int)currentCell.x, (int)currentCell.y);
            Vector2Int nextCell = new Vector2Int(currentCellInt.x + (int)direction.x, currentCellInt.y + (int)direction.y);

            if (IsWithinGridBounds(nextCell))
            {
                Vector2 targetCellCenter = levelGenerator.gridManager.GetWorldPosition(nextCell.x, nextCell.y) +
                                           levelGenerator.gridManager.GetCellCenterOffset();

                targetPosition = new Vector3(targetCellCenter.x - 1, targetCellCenter.y - 1, -5f);
                isMoving = true;
            }
            else
            {
                ChangeDirection(currentCell);
            }
        }
        else
        {
            ChangeDirection(currentCell);
        }
    }

    void ToggleSpikesBasedOnColor()
    {
        CrystalManager crystalManager = FindObjectOfType<CrystalManager>();

        bool isSafe = crystalManager.IsColorSafe(currentCellColor);

        if (spikes != null)
        {
            spikes.SetActive(!isSafe);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColorCrystal"))
        {
            Color crystalColor = collision.GetComponent<SpriteRenderer>().color;
            Debug.Log("Color crystal gathered: " + crystalColor);
            crystalManager.RemoveColorCrystal(collision.gameObject);
            // Implement logic based on the color gathered
        }

        if (collision.CompareTag("ProgressCrystal"))
        {
            Color crystalColor = collision.GetComponent<SpriteRenderer>().color;
            Debug.Log("Progress crystal gathered: " + crystalColor);
            crystalManager.RemoveProgressCrystal(collision.gameObject);
            // Implement logic based on the progress gathered
        }

    }

    void ChangeDirection(Vector2 currentCell)
    {
        Vector2Int currentCellInt = new Vector2Int((int)currentCell.x, (int)currentCell.y);
        direction *= -1;
        Vector2Int nextCell = new Vector2Int(currentCellInt.x + (int)direction.x, currentCellInt.y + (int)direction.y);

        if (IsWithinGridBounds(nextCell))
        {
            Vector2 targetCellCenter = levelGenerator.gridManager.GetWorldPosition(nextCell.x, nextCell.y) +
                                       levelGenerator.gridManager.GetCellCenterOffset();

            targetPosition = new Vector3(targetCellCenter.x - 1, targetCellCenter.y - 1, -5f);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    bool CanMoveInDirection(Vector2 cell, Vector2 dir)
    {
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


