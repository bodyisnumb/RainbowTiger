using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Button tutorialButton;
    public GameObject backgroundImage;
    public GameObject mainButtonsPanel;
    public Text[] tutorialTexts;
    private int currentIndex = -1;
    private bool tutorialStarted = false;

    void Start()
    {
        backgroundImage.SetActive(false);

        foreach (Text text in tutorialTexts)
        {
            text.gameObject.SetActive(false);
        }

        tutorialButton.onClick.AddListener(StartTutorial);
    }

    void StartTutorial()
    {
        tutorialButton.gameObject.SetActive(false);
        tutorialStarted = true;
        ShowNextTutorial();
    }

    void Update()
    {
        if (tutorialStarted && currentIndex >= 0 && Input.GetMouseButtonDown(0))
        {
            ShowNextTutorial();
        }
    }

    void ShowNextTutorial()
    {
        currentIndex++;

        if (currentIndex < tutorialTexts.Length)
        {
            backgroundImage.SetActive(true);

            foreach (Text text in tutorialTexts)
            {
                text.gameObject.SetActive(false);
            }

            tutorialTexts[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            backgroundImage.SetActive(false);
            tutorialTexts[2].gameObject.SetActive(false);
            mainButtonsPanel.SetActive(true);
            Debug.Log("Tutorial Ended. Perform necessary actions...");
            tutorialButton.gameObject.SetActive(true);
        }
    }
}
