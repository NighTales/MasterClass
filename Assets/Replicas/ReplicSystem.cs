using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReplicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private SubsPanel subsPanel;
    [SerializeField] private KeyCode skipButton = KeyCode.Q;

    private List<ReplicaPack> replicaPacks;
    private PlayerUI playerUI;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        replicaPacks = new List<ReplicaPack>();
        subsPanel.ClosePanel();
    }

    public void SetUp()
    {
        playerUI.pauseStateChanged.AddListener(OnChangePause);
        SettingsHolder.VoiceVolumeChanged.AddListener(OnChangeVolume);
        SettingsHolder.UseSubsChanged.AddListener(OnDrawSubsChanged);
    }

    private void Update()
    {
        if(Input.GetKeyDown(skipButton))
        {
            SkipAll();
        }
    }

    public void AddNewReplicaPack(ReplicaPack pack)
    {
        replicaPacks.Add(pack);
        if(replicaPacks.Count == 1)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
    }

    public void OnDrawSubsChanged(bool value)
    {
        if(value)
        {
            if(replicaPacks.Count > 0)
            {
                ReplicaItem item = replicaPacks[0].mainList[0];
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
        }
        else
        {
            subsPanel.ClosePanel();
        }
    }

    public void OnChangePause(bool value)
    {
        if(value)
        {
            voiceAudioSource.Pause();
        }
        else
        {
            voiceAudioSource.UnPause();
        }
    }

    private void OnChangeVolume(float newVolume)
    {
        voiceAudioSource.volume = newVolume;
    }

    private void SkipAll()
    {
        if(replicaPacks.Count <= 0)
        {
            return;
        }

        StopAllCoroutines();

        if (voiceAudioSource.isPlaying)
            voiceAudioSource.Stop();
        subsPanel.ClosePanel();

        foreach (var item in replicaPacks[0].mainList)
        {
            if (!item.onReplicaStart.isDecorEvent)
            {
                item.onReplicaStart.action.Invoke();
            }
            if (!item.onReplicaEnd.isDecorEvent)
            {
                item.onReplicaEnd.action.Invoke();
            }
        }

        replicaPacks[0].onSkipAll.Invoke();

        replicaPacks.RemoveAt(0);

        if (replicaPacks.Count > 0)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
    }

    private IEnumerator TallMainReplicasCoroutine()
    {
        subsPanel.SetSkipTip(skipButton);
        while (replicaPacks[0].mainList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].mainList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if(SettingsHolder.UseSubs)
            {
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
            item.onReplicaStart.action.Invoke();

            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].mainList.RemoveAt(0);
            item.onReplicaEnd.action.Invoke();
            yield return new WaitForSeconds(0.3f);
        }

        replicaPacks.RemoveAt(0);

        yield return null;

        if(replicaPacks.Count > 0)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
        else
        {
            subsPanel.ClosePanel();
            subsPanel.HideSkipTip();
        }
    }
}

[System.Serializable]
public class ReplicaPack
{
    public List<ReplicaItem> mainList;
    public UnityEvent onSkipAll;
}

[System.Serializable]
public class ReplicaItem
{
    public string CharacterName;
    public Color characterColor = Color.white;
    [TextArea]
    public string characterText;
    public AudioClip characterVoice;
    public UnityEventPack onReplicaStart;
    public UnityEventPack onReplicaEnd;
}

[System.Serializable]
public class UnityEventPack
{
    public bool isDecorEvent = false;
    public UnityEvent action;
}

[System.Serializable]
public class SubsPanel
{
    public GameObject panelObject;
    public GameObject skipTipObject;
    public Text characterName;
    public Text characterText;
    public Text skipTip;

    public void SetSubs(string name, string text, Color color)
    {
        panelObject.SetActive(true);
        characterName.color = color;
        characterName.text = name;
        characterText.text = text;
    }

    public void ClosePanel()
    {
        characterName.text = characterText.text = string.Empty;
        panelObject.SetActive(false);
    }
    public void SetSkipTip(KeyCode skipKey)
    {
        skipTipObject.SetActive(true);
        skipTip.text = skipKey.ToString() + " - заткнуть собеседника";
    }
    public void HideSkipTip()
    {
        skipTipObject.SetActive(false);
    }
}
