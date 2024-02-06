using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public GameObject mainButtonsPanel;

    void Start()
    {
        // Check if the current scene is the "MainMenu" scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Start the initial loading screen coroutine
            StartCoroutine(ShowInitialLoadingScreen());
        }
    }

    IEnumerator ShowInitialLoadingScreen()
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Set the initial delay to 7.5 seconds
        float initialDelay = 7.5f;
        float elapsedTime = 0f;

        // Wait for the initial delay
        while (elapsedTime < initialDelay)
        {
            elapsedTime += Time.deltaTime;

            // Update loading bar progress during initial delay
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Clamp01(elapsedTime / initialDelay);
            }

            yield return null;
        }

        // If MainButtonsPanel is found, you can access its components or modify its properties
        if (mainButtonsPanel != null)
        {
            mainButtonsPanel.SetActive(true);
        }
        // Load the main menu scene
        loadingScreen.SetActive(false);
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync("Game"));
    }

    public void LoadMenuScene()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }

    public void LoadMenuFast()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTutorialFast()
    {
        SceneManager.LoadScene("Tutorial");
    }

 IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        
        float initialDelay = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < initialDelay)
        {
            elapsedTime += Time.deltaTime;

            
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Lerp(0f, 0.5f, elapsedTime / initialDelay);
            }

            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); 

            
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Lerp(0.5f, 1f, progress);
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}




