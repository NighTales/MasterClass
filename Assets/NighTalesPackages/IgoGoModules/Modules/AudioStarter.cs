using UnityEngine;

/// <summary>
/// Запускает звук
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioStarter : UsingObject
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public override void ToStart()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public override void Use()
    {
        if(source.loop)
        {
            if (source.isPlaying)
            {
                source.Stop();
                return;
            }
        }
        source.Play();
    }
}
