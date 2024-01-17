using UnityEngine;

public class ShieldPower : MonoBehaviour
{
    public float shieldDuration = 5f; // Adjust the shield duration as needed
    private bool isShieldActive = false;
    private float shieldTimer = 0f;
    private TigerMovement tigerMovement;


    // Call this method when the Shield Power button is pressed
    public void ActivateShieldPower()
    {
        if (!isShieldActive)
        {
            isShieldActive = true;
            shieldTimer = 0f;
        //sound
        }
    }

    private void Update()
    {
        tigerMovement = FindObjectOfType<TigerMovement>();


        if (isShieldActive)
        {
            tigerMovement.isShieldActive = true;
            shieldTimer += Time.deltaTime;

            if (shieldTimer >= shieldDuration)
            {
                DeactivateShieldPower();
            }
        }
    }

    private void DeactivateShieldPower()
    {
        isShieldActive = false;
        tigerMovement.isShieldActive = false;
    }
}

