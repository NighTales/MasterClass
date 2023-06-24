using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumechanger : MonoBehaviour
{
    [SerializeField]
    private SoundType soundType;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        switch (soundType)
        {
            case SoundType.Voice:
                SettingsHolder.VoiceVolumeChanged.AddListener(OnVolumeChanged);
                source.volume = SettingsHolder.Voice;
                break;
            case SoundType.Music:
                SettingsHolder.MusicVolumeChanged.AddListener(OnVolumeChanged);
                source.volume = SettingsHolder.Music;
                break;
            case SoundType.Effect:
                SettingsHolder.SoundsVolumeChanged.AddListener(OnVolumeChanged);
                source.volume = SettingsHolder.Effects;
                break;

        }

    }

    public void OnVolumeChanged(float volume)
    {
        source.volume = volume;
    }
}

public enum SoundType
{
    Music,
    Voice,
    Effect
}
