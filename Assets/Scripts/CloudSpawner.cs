using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs; 
    public float spawnInterval = 5f;
    public float spawnHeight = 5f;
    public float fixedXOffset = 10f; 

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        InvokeRepeating("SpawnCloud", 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        if (mainCamera == null)
            return;

       
        GameObject selectedCloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        float randomY = Random.Range(-5f, 5f); 

        
        Vector3 spawnPosition = mainCamera.transform.position + new Vector3(fixedXOffset, randomY, 2f);

        GameObject cloud = Instantiate(selectedCloudPrefab, spawnPosition, Quaternion.identity);
        
    }
}


