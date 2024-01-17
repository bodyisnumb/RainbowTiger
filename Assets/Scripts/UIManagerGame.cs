using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
    public Text magnetCountText;
    public Text shieldCountText;

    private EconomicManager economicManager;
    public int currentScore = 0;

    private void Start()
    {
        economicManager = EconomicManager.instance;

        UpdateUI();
    }

    public void UpdateUI()
    {
        magnetCountText.text = economicManager.GetBatteryCount().ToString();
        shieldCountText.text = economicManager.GetShieldCount().ToString();
    }

    public void AddToScoreAndCoins(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        economicManager.AddCoins(scoreToAdd);
        UpdateUI();
    }
}
