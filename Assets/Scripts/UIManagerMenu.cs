using UnityEngine;
using UnityEngine.UI;

public class UIManagerMenu : MonoBehaviour
{
    public Text coinText1;
    public Text coinText2;
    public Text magnetsCountText;
    public Text shieldCountText;

    public Button buyMagnetsButton;
    public Button buyShieldButton;

    private EconomicManager economicManager;

    private void Start()
    {
        economicManager = EconomicManager.instance;

        buyMagnetsButton.onClick.AddListener(BuyMagnets);
        buyShieldButton.onClick.AddListener(BuyShield);

        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText1.text = economicManager.GetCoinCount().ToString();
        coinText2.text = economicManager.GetCoinCount().ToString();
        magnetsCountText.text = "Magnets:" + economicManager.GetBatteryCount().ToString();
        shieldCountText.text = "Shields:" + economicManager.GetShieldCount().ToString();
    }

    private void BuyMagnets()
    {
        if (economicManager.DeductCoins(25))
        {
            economicManager.AddBattery();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy magnets!");
        }
    }

    private void BuyBomb()
    {
        if (economicManager.DeductCoins(10))
        {
            economicManager.AddBomb();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy bomb!");
        }
    }

    private void BuyShield()
    {
        if (economicManager.DeductCoins(50))
        {
            economicManager.AddShield();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy shield!");
        }
    }
}
