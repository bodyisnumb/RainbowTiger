using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs; // Array to hold different cloud prefabs
    public float spawnInterval = 5f;
    public float spawnHeight = 5f;
    public float fixedXOffset = 10f; // Set the fixed x-offset from the camera's position

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

        // Randomly select a cloud prefab from the array
        GameObject selectedCloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        float randomY = Random.Range(-5f, 5f); // Adjust the range based on your scene

        // Use the camera's current position as a reference for spawning
        Vector3 spawnPosition = mainCamera.transform.position + new Vector3(fixedXOffset, randomY, 2f);

        GameObject cloud = Instantiate(selectedCloudPrefab, spawnPosition, Quaternion.identity);
        // Optionally, you can add additional settings or components to the cloud object.
    }
}


