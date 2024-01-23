using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{



    public AudioSource audioSource;
    public AudioClip button;
    public AudioClip battery_recharging;
    public AudioClip bush;
    public AudioClip electricity;
    public AudioClip shield_guard;
    public AudioClip success;

    public static SoundPlayer SoundControllerInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (SoundControllerInstance == null)
        {
            SoundControllerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ButtonSound()
    {
        audioSource.clip = button;
        audioSource.Play();
    }

    public void BatterySound()
    {
        audioSource.clip = battery_recharging;
        audioSource.Play();
    }

    public void BushSound()
    {
        audioSource.clip = bush;
        audioSource.Play();
    }

    public void BombSound()
    {
        audioSource.clip = electricity;
        audioSource.Play();
    }

    public void ShieldSound()
    {
        audioSource.clip = shield_guard;
        audioSource.Play();
    }

    public void SuccessSound()
    {
        audioSource.clip = success;
        audioSource.Play();
    }

}
