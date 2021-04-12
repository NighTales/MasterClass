using System.Collections;
using UnityEngine;

/// <summary>
/// ����� ������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class MusicMixer : UsingObject
{
    [SerializeField, Tooltip("���������, ������� ������ �����������")] private AudioClip newMusic;

    [SerializeField, Tooltip("�������� �����")] private AudioSource source;

    [SerializeField, Tooltip("�������� Loop ����� �����������")] private bool loop =true;

    [SerializeField, Tooltip("������ ����� ��� ����������")] private bool hardChange = false;

    public override void ToStart()
    {
    }

    public override void Use()
    {
        if (hardChange)
        {
            ChangeClip();
        }
        else
        {
            StartCoroutine(ChangeMusicCoroutine());
        }
    }

    private IEnumerator ChangeMusicCoroutine()
    {
        while(source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }
        source.volume = 0;
        ChangeClip();
        while (source.volume < 1)
        {
            source.volume += Time.deltaTime;
            yield return null;
        }
        source.volume = 1;
    }
    private void ChangeClip()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
        source.clip = newMusic;
        source.Play();
        source.loop = loop;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            if(source == null)
            {
                Debug.LogError("�� ������� �������� ����� � " + gameObject.name);
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, source.transform.position);
        }
    }
}
