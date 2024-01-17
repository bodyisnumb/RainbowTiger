using UnityEngine;
using UnityEngine.UI;

public class AudioMuter : MonoBehaviour
{
    public AudioSource audioSource;
    public Toggle muteToggle;

    private const string MUTE_PREF_KEY = "IsMuted";

    private void Start()
    {
        if (audioSource != null && muteToggle != null)
        {
            if (PlayerPrefs.HasKey(MUTE_PREF_KEY))
            {
                bool isMuted = PlayerPrefs.GetInt(MUTE_PREF_KEY) == 1;
                audioSource.mute = isMuted;
                muteToggle.isOn = isMuted;
            }
            else
            {
                PlayerPrefs.SetInt(MUTE_PREF_KEY, audioSource.mute ? 1 : 0);
            }

            muteToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isMuted)
    {
        if (audioSource != null)
        {
            audioSource.mute = isMuted;

            PlayerPrefs.SetInt(MUTE_PREF_KEY, isMuted ? 1 : 0);
        }
    }
}
