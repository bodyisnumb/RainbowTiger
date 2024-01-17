using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float transitionDuration = 1f; // Adjust the transition duration as needed
    public float transitionOffset = 0.25f; // Adjust the offset to start the transition earlier

    private Transform target;
    private bool transitioning = false;

    void Update()
    {
        GameObject tiger = GameObject.FindWithTag("Tiger");
        if (tiger != null)
        {
            target = tiger.transform;

            if (!transitioning)
            {
                float halfScreenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

                // Check if the tiger is approaching the edge of the screen
                if (Mathf.Abs(target.position.x - transform.position.x) > halfScreenWidth - transitionOffset)
                {
                    Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    StartCoroutine(TransitionToPosition(targetPosition));
                }
            }
        }
        else
        {
            Debug.LogError("Tiger not found. Make sure the tiger is tagged appropriately.");
        }
    }

    IEnumerator TransitionToPosition(Vector3 targetPosition)
    {
        transitioning = true;

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure a smooth final transition to the exact target position
        transform.position = targetPosition;

        transitioning = false;
    }
}





