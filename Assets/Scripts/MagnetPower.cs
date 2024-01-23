using UnityEngine;

public class MagnetPower : MonoBehaviour
{
    public float magnetDuration = 1f; // Adjust the duration as needed
    public float enlargedColliderRadius = 8f; // Adjust the enlarged collider size as needed
    private TigerMovement tigerMovement;
    private EconomicManager economicManager;
    private SoundPlayer soundPlayer;
    private bool isMagnetActive = false;
    private float magnetTimer = 0f;
    private int magnetCount = 0;

    void Start()
    {
        economicManager = FindObjectOfType<EconomicManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();

    }

    // Call this method when the Magnet Power button is pressed
    public void ActivateMagnetPower()
    {
        if (!isMagnetActive && magnetCount > 0)
        {
            isMagnetActive = true;
            magnetTimer = 0f;

            soundPlayer.BatterySound();
            tigerMovement.EnlargeCollider(enlargedColliderRadius);
            economicManager.DeductBattery();
            // Implement any visual or sound effects for the magnet activation
        }
    }

    private void Update()
    {
        tigerMovement = FindObjectOfType<TigerMovement>();

        magnetCount = economicManager.GetBatteryCount();

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

