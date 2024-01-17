using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pawPanel;
    public GameObject powerPanel;
    private Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        GetComponent<Button>().onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
            DisablePanels();
            buttonText.text = "Paused";
        }
        else
        {
            UnpauseGame();
            EnablePanels();
            buttonText.text = "";
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    private void DisablePanels()
    {
        if (pawPanel != null)
        {
            pawPanel.SetActive(false);
        }

        if (powerPanel != null)
        {
            powerPanel.SetActive(false);
        }
    }

    private void EnablePanels()
    {
        if (pawPanel != null)
        {
            pawPanel.SetActive(true);
        }

        if (powerPanel != null)
        {
            powerPanel.SetActive(true);
        }
    }
}
