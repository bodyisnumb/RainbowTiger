using UnityEngine;

public class MagnetPower : MonoBehaviour
{
    public float magnetDuration = 1f; 
    public float enlargedColliderRadius = 8f; 
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

    
    public void ActivateMagnetPower()
    {
        if (!isMagnetActive && magnetCount > 0)
        {
            isMagnetActive = true;
            magnetTimer = 0f;

            soundPlayer.PlaySound("Battery");
            tigerMovement.EnlargeCollider(enlargedColliderRadius);
            economicManager.DeductBattery();
            
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

        
        tigerMovement.ShrinkCollider();
        

        
    }
}

