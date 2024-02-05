using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
