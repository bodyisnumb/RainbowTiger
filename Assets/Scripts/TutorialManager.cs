using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public string[] tutorialTexts;  
    public Text tutorialTextComponent;

    private int currentIndex = 0;

    void Start()
    {
        
        Debug.Log("Array Length: " + tutorialTexts.Length);

        if (tutorialTexts.Length > 0)
        {
            tutorialTextComponent.text = tutorialTexts[currentIndex];
        }
        else
        {
            Debug.LogError("TutorialManager: Ensure tutorialTexts array is not empty.");
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            if (currentIndex < tutorialTexts.Length - 1)
            {
                
                currentIndex++;
                tutorialTextComponent.text = tutorialTexts[currentIndex];
            }
            else
            {
                
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}



