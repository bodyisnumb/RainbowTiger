using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController MusicControllerInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (MusicControllerInstance == null)
        {
            MusicControllerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
