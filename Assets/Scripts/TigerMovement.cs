using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    private float unsafeTime = 0f;
//    private const float maxUnsafeTime = 0.01f;
    private const float maxUnsafeTime = 10f; //undead
    private float moveSpeed = 2f;
    private LevelGenerator levelGenerator;
    private CrystalManager crystalManager;
    private GridManager gridManager;
    private UIManagerGame uIManagerGame;
    private Color currentCellColor;
    private Vector2 direction;
    private bool isMoving;
    private Vector3 targetPosition;

    private CircleCollider2D tigerCollider;
    private float originalColliderRadius;

    public bool isShieldActive = false;

    private int gridWidth;
    private int gridHeight;
    private bool gameStarted;

    public GameObject spikes;

    public GameObject explosionPrefab;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        crystalManager = FindObjectOfType<CrystalManager>();
        gridManager = FindObjectOfType<GridManager>();
        uIManagerGame = FindObjectOfType<UIManagerGame>();

        tigerCollider = GetComponent<CircleCollider2D>();

        if (tigerCollider != null)
        {
            originalColliderRadius = tigerCollider.radius;
        }

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
            CheckUnsafeColor();
        }

        UpdateCurrentCellColor();
        Debug.Log(unsafeTime);
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
                    // Instantiate the explosion prefab
            GameObject explosionInstance = Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);

            // Get the particle system component on the explosion prefab
            ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();
            
            // Play the particle system
            if (explosionParticles != null)
            {
                explosionParticles.Play();
            }
            crystalManager.RemoveColorCrystal(collision.gameObject);
            // Implement logic based on the color gathered
            Destroy(explosionInstance, explosionParticles.main.duration);
            uIManagerGame.AddToScoreAndCoins(1);
        }

        if (collision.CompareTag("ProgressCrystal"))
        {
            Color crystalColor = collision.GetComponent<SpriteRenderer>().color;
            Debug.Log("Progress crystal gathered: " + crystalColor);
            GameObject explosionInstance = Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);

            // Get the particle system component on the explosion prefab
            ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();
            
            // Play the particle system
            if (explosionParticles != null)
            {
                explosionParticles.Play();
            }
            crystalManager.RemoveProgressCrystal(collision.gameObject);
            // Implement logic based on the progress gathered
            Destroy(explosionInstance, explosionParticles.main.duration);
            uIManagerGame.AddToScoreAndCoins(1);
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

    void CheckUnsafeColor()
    {
        if(!isShieldActive)
        {
            if (!crystalManager.IsColorSafe(currentCellColor))
            {
                unsafeTime += Time.deltaTime;

                if (unsafeTime >= maxUnsafeTime)
                {
                    GameOver();
                    Debug.Log("Game Over");
                }
            }
            else
            {
                unsafeTime = 0f;
            }
        }
    }

    public void EnlargeCollider(float newRadius)
    {
        if (tigerCollider != null)
        {
            tigerCollider.radius = newRadius;
        }
    }

    // Add this method to shrink the collider back to its original size
    public void ShrinkCollider()
    {
        if (tigerCollider != null)
        {
            // Set the collider back to its original size, you may need to adjust this based on your initial collider radius
            tigerCollider.radius = originalColliderRadius;
        }
    }

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
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


