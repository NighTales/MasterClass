using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeChanger : MonoBehaviour
{
    public event Action<AudioSource, float> MusicVolumeChanged;

    public AudioType type;
    [HideInInspector] public float maxValue = 1f;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        try
        {
            AudioCenter audioCenter = GameObject.FindGameObjectWithTag("AudioCenter").GetComponent<AudioCenter>();
            switch (type)
            {
                case AudioType.music:
                    audioCenter.MusicVolumeChanged += SetValue;
                    SetValue(audioCenter.MusicVolume);
                    break;
                case AudioType.sounds:
                    audioCenter.SoundsVolumeChanged += SetValue;
                    SetValue(audioCenter.SoundVolume);
                    break;
                default:
                    audioCenter.MusicVolumeChanged += SetValue;
                    break;
            }
        }
        catch(NullReferenceException)
        {
            Debug.LogError("Объект с настройками громкости звука не найден");
        }
        
    }

    public void SetValue(float newWalue)
    {
        maxValue = newWalue;
        if (type == AudioType.sounds)
            source.volume = maxValue;
        else
            MusicVolumeChanged?.Invoke(source, maxValue);
    }
}

public enum AudioType
{
    music,
    sounds
}


//это ПРИМЕР класса, в котором меняются настройки звука. Чаще всего события вызываются из слайдеров в настройках
public class AudioCenter : MonoBehaviour
{
    public float MusicVolume { get; set; } //можно ссылать на слайдеры или значения playerPrefs
    public float SoundVolume { get; set; }

    public Action<float> MusicVolumeChanged;
    public Action<float> SoundsVolumeChanged;
}

