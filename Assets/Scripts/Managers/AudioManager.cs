using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SavedSettings _settings;
    
    void Start()
    {
        SettingsManager.Instance.updateAudio += UpdateVolume;

        AudioListener.volume = _settings.MasterVolume;
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.updateAudio -= UpdateVolume;
    }

    private void UpdateVolume(SettingsManager.AudioSetting audioSetting, float volume)
    {
        if (audioSetting.Equals(SettingsManager.Instance.MasterVolume))
        {
            AudioListener.volume = _settings.MasterVolume;
        }
        else
        {
            foreach(SettingsManager.AudioSetting.AudioStruct audioStruct in audioSetting.Structs)
            {
                audioStruct.Source.volume = audioStruct.Volume * volume;
            }
        }
    }
}
