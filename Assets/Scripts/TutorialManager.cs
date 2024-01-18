using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public string[] tutorialTexts;  // Array of tutorial texts corresponding to each step
    private Text tutorialTextComponent;

    private int currentIndex = 0;

    void Start()
    {
        if (tutorialTexts.Length == 0)
        {
            Debug.LogError("TutorialManager: Ensure tutorialImage is set and tutorialTexts array is not empty.");
            return;
        }

        tutorialTextComponent.text = tutorialTexts[currentIndex];
    }

    void Update()
    {
        // Check for touch/click input to proceed through the tutorial
        if (Input.GetMouseButtonDown(0))
        {
            // Check if there are more tutorial steps
            if (currentIndex < tutorialTexts.Length - 1)
            {
                // Move to the next tutorial step
                currentIndex++;
                tutorialTextComponent.text = tutorialTexts[currentIndex];
            }
            else
            {
                // Load the main menu scene when the tutorial is complete
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}


