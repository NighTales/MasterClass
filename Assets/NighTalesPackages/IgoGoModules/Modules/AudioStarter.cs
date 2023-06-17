using UnityEngine;

/// <summary>
/// Запускает звук
/// </summary>
[RequireComponent(typeof(AudioSource))]
[HelpURL("https://docs.google.com/document/d/15bMqeC8_jUhC7jlFw8qPEHcc5hBNMDnmTguAbIdNcUk/edit?usp=sharing")]
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
