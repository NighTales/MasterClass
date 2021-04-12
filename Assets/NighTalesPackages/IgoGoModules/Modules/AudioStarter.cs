using UnityEngine;

/// <summary>
/// Запускает звук
/// </summary>
[RequireComponent(typeof(AudioSource))]
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
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
