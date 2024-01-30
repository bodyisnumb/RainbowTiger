using UnityEngine;

public class ShieldPower : MonoBehaviour
{
    public float shieldDuration = 5f; // Adjust the shield duration as needed
    private bool isShieldActive = false;
    private float shieldTimer = 0f;
    private TigerMovement tigerMovement;
    private EconomicManager economicManager;
    private SoundPlayer soundPlayer;
    private int shieldCount = 0;

    void Start()
    {
        economicManager = FindObjectOfType<EconomicManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();

    }

    // Call this method when the Shield Power button is pressed
    public void ActivateShieldPower()
    {
        if (!isShieldActive && shieldCount > 0)
        {
            isShieldActive = true;
            shieldTimer = 0f;
            economicManager.DeductShield();
            soundPlayer.PlaySound("Shield");
        }
    }

    private void Update()
    {
        tigerMovement = FindObjectOfType<TigerMovement>();

        shieldCount = economicManager.GetShieldCount();

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

