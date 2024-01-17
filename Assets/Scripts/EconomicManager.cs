using UnityEngine;

public class EconomicManager : MonoBehaviour
{
    public static EconomicManager instance;

    private int coinCount = 0;
    private int batteryCount = 0;
    private int bombCount = 0;
    private int shieldCount = 0;

    private const string COIN_KEY = "CoinCount";
    private const string BATTERY_KEY = "BatteryCount";
    private const string BOMB_KEY = "BombCount";
    private const string SHIELD_KEY = "ShieldCount";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadCounts();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        SaveCounts();
    }

    public bool DeductCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            SaveCounts();
            return true;
        }
        return false;
    }

    public void AddBattery()
    {
        batteryCount++;
        SaveCounts();
    }

    public void AddBomb()
    {
        bombCount++;
        SaveCounts();
    }

    public void AddShield()
    {
        shieldCount++;
        SaveCounts();
    }

    public void DeductBattery()
    {
        batteryCount--;
        SaveCounts();
    }

    public void DeductBomb()
    {
        bombCount--;
        SaveCounts();
    }

    public void DeductShield()
    {
        shieldCount--;
        SaveCounts();
    }

    private void SaveCounts()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.SetInt(BATTERY_KEY, batteryCount);
        PlayerPrefs.SetInt(BOMB_KEY, bombCount);
        PlayerPrefs.SetInt(SHIELD_KEY, shieldCount);
        PlayerPrefs.Save();
    }

    private void LoadCounts()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
        batteryCount = PlayerPrefs.GetInt(BATTERY_KEY, 0);
        bombCount = PlayerPrefs.GetInt(BOMB_KEY, 0);
        shieldCount = PlayerPrefs.GetInt(SHIELD_KEY, 0);
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public int GetBatteryCount()
    {
        return batteryCount;
    }

    public int GetBombCount()
    {
        return bombCount;
    }

    public int GetShieldCount()
    {
        return shieldCount;
    }
}

