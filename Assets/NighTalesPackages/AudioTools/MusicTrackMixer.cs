using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrackMixer : MonoBehaviour
{
    [Tooltip("Скорость смены музыки"), Range(0.01f, 1)] public float changeSpeed;
    public AudioSource mainAudioSource;
    public AudioSource supportAudioSource;

    private bool changeKey = false;
    private float currentMaxVolumeForMainSource = 1;

    private void Start()
    {
        InitializeManager();
    }
    private void Update()
    {
        MixMusic();
    }

    /// <summary>
    /// этот метод задаёт стартовые установки. Если нужно как-то по особому управлять инициализацией скрипта, то менять нужно именно этот метод.
    /// </summary>
    public void InitializeManager()
    {
        changeKey = false;
        mainAudioSource.playOnAwake = supportAudioSource.playOnAwake = false;
        mainAudioSource.loop = supportAudioSource.loop = true;

        mainAudioSource.GetComponent<AudioVolumeChanger>().MusicVolumeChanged += CheckMusicVolume;
        supportAudioSource.GetComponent<AudioVolumeChanger>().MusicVolumeChanged += CheckMusicVolume;

        supportAudioSource.Stop();
        supportAudioSource.volume = 0;
    }

    /// <summary>
    /// Устанавливает новый активный музыкальный трек
    /// </summary>
    /// <param name="clip"></param>
    private void ChangeMusic(AudioClip clip)
    {
        if(mainAudioSource.clip != clip)
        {
            AudioSource bufer = mainAudioSource;
            mainAudioSource = supportAudioSource;
            supportAudioSource = bufer;
            mainAudioSource.clip = clip;
            mainAudioSource.Play();
            changeKey = true;
        }
    }
    /// <summary>
    /// Плавно миксует старый активный музыкальный трек и новый
    /// </summary>
    private void MixMusic()
    {
        if(changeKey)
        {
            mainAudioSource.volume += Time.deltaTime/2;
            supportAudioSource.volume -= Time.deltaTime/2;

            if(mainAudioSource.volume >= currentMaxVolumeForMainSource && supportAudioSource.volume <= 0)
            {
                mainAudioSource.volume = currentMaxVolumeForMainSource;
                supportAudioSource.volume = 0;
                supportAudioSource.Stop();
                changeKey = false;
            }
        }
    }
    /// <summary>
    /// Метод используется для изменения уровня звука музыки с помощью AudioVolumeChanger (в данном случае будет учитываться момент микширования)
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newValue"></param>
    private void CheckMusicVolume(AudioSource source, float newValue)
    {
        if(mainAudioSource == source)
        {
            currentMaxVolumeForMainSource = newValue;
            if(changeKey)
            {
                if (mainAudioSource.volume > newValue)
                    mainAudioSource.volume = newValue;
            }
            else
                mainAudioSource.volume = newValue;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MusicChanger"))
        {
            ChangeMusic(other.GetComponent<MusicChanger>().CheckChange());
        }
    }
}
