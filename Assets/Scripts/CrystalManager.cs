using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class CrystalManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> colorCrystals = new List<GameObject>();
    [SerializeField]
    private List<GameObject> progressCrystals = new List<GameObject>();

    public GameObject safeColorIndicator;

    private bool wonDisplayed = false;
    private Color safeColor = Color.white;

    void Update()
    {
        if (progressCrystals.Count == 0 && !wonDisplayed)
        {
            Debug.Log("YOU WON!");
            wonDisplayed = true;
        }
    }

    public void AddColorCrystal(GameObject crystal)
    {
        colorCrystals.Add(crystal);
    }

    public void AddProgressCrystal(GameObject crystal)
    {
        progressCrystals.Add(crystal);
    }

    public void RemoveColorCrystal(GameObject crystal)
    {
        if (colorCrystals.Contains(crystal))
        {
            ChangeSafeColor(crystal.GetComponent<SpriteRenderer>().color);

            colorCrystals.Remove(crystal);
            Destroy(crystal);
        }
    }

    public void RemoveProgressCrystal(GameObject crystal)
    {
        if (progressCrystals.Contains(crystal))
        {
            progressCrystals.Remove(crystal);
            Destroy(crystal);
        }
    }

    public void ChangeSafeColor(Color newColor)
    {
        safeColor = newColor;
        UpdateSafeColorIndicator();
    }

    public bool IsColorSafe(Color colorToCheck)
    {
        return colorToCheck == safeColor;
    }

    void UpdateSafeColorIndicator()
    {
        if (safeColorIndicator != null)
        {
            SpriteRenderer indicatorRenderer = safeColorIndicator.GetComponent<SpriteRenderer>();
            if (indicatorRenderer != null)
            {
                indicatorRenderer.color = safeColor;
            }
        }
    }
}

