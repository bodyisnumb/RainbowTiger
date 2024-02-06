using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pawPanel;
    public GameObject powerPanel;
    public GameObject pausePanel;
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
            if (pausePanel != null)
                {
                    pausePanel.SetActive(true);
                }
        //    buttonText.text = "Paused";
        }
        else
        {
            UnpauseGame();
            EnablePanels();
            if (pausePanel != null)
                {
                    pausePanel.SetActive(false);
                }
        //    buttonText.text = "";
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
