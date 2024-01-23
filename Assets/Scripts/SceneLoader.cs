using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;

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

        // Add a minimum loading time for the initial 50%
        float initialDelay = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < initialDelay)
        {
            elapsedTime += Time.deltaTime;

            // Update the loading bar value during the initial delay
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Lerp(0f, 0.5f, elapsedTime / initialDelay);
            }

            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 0.9 is the completion point

            // Update the loading bar value after the initial delay
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Lerp(0.5f, 1f, progress);
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}



