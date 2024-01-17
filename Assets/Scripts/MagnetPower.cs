using UnityEngine;

public class MagnetPower : MonoBehaviour
{
    public float magnetDuration = 3f; // Adjust the duration as needed
    public float enlargedColliderRadius = 8f; // Adjust the enlarged collider size as needed
    private TigerMovement tigerMovement;
    private bool isMagnetActive = false;
    private float magnetTimer = 0f;


    // Call this method when the Magnet Power button is pressed
    public void ActivateMagnetPower()
    {
        if (!isMagnetActive)
        {
            isMagnetActive = true;
            magnetTimer = 0f;

            // Enlarge the tiger's collider
            tigerMovement.EnlargeCollider(enlargedColliderRadius);

            // Implement any visual or sound effects for the magnet activation
        }
    }

    private void Update()
    {
        tigerMovement = FindObjectOfType<TigerMovement>();

        if (isMagnetActive)
        {
            magnetTimer += Time.deltaTime;

            if (magnetTimer >= magnetDuration)
            {
                DeactivateMagnetPower();
            }
        }
    }

    private void DeactivateMagnetPower()
    {
        isMagnetActive = false;

        // Shrink the tiger's collider back to its original size
        tigerMovement.ShrinkCollider();

        // Implement any visual or sound effects for the magnet deactivation
    }
}

