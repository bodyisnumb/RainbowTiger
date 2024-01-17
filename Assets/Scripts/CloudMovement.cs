using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        // Move the cloud to the left (negative X direction)
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Optionally, destroy the cloud when it goes off-screen
        if (transform.position.x < -20f) // Adjust the threshold based on your scene
        {
            Destroy(gameObject);
        }
    }
}
