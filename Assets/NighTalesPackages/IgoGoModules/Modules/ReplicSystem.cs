using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Replic
{
    [Tooltip("Реплика")]
    public string text;
    [Tooltip("Цвет текста")]
    public Color color;
    [Tooltip("Аудиоклип реплики")]
    public AudioClip clip;
}


/// <summary>
/// Этот модуль запускает набор реплик без ограничения управления. А после того, как реплики кончатся, может дать сигнал другим модулям. Одноразовый модуль!
/// </summary>
[RequireComponent(typeof (AudioSource))]
public class ReplicSystem : UsingOrigin
{
	[Tooltip("Панель UI, в которой показываются субтитры")]
    public GameObject subsPanel;
    [Tooltip("UI TEXT, где будут отображаться субтитры")]
    public Text subs;
    [Tooltip("Играть при запуске сцены")]
    public bool playOnAwake;
    [Space(20)]
    public Replic[] replics;

    private AudioSource source;
    private int currentNumber;

    void Start()
    {
        source = GetComponent<AudioSource>();
		
        ToStart();

		if(playOnAwake)
		{
			Use();
		}
    }

    private void StartReplic(int number)
    {
        currentNumber = number;
        subs.color = replics[number].color;
        subs.text = replics[number].text;
        source.clip = replics[number].clip;
        Invoke("Next", replics[number].clip.length + 0.5f);
    }
    private void Next()
    {
        if (source.isPlaying)
            source.Stop();
        currentNumber++;
        if (currentNumber >= replics.Length)
        {
            subsPanel.SetActive(false);
            UseAll();
            ClearMeFromActors();
            Destroy(gameObject);
        }
        else
        {
            StartReplic(currentNumber);
            source.Play();
            currentNumber++;
        }
    }

    public override void Use()
    {
        subsPanel.SetActive(true);
        StartReplic(0);
    }
    public override void ToStart()
    {
        used = false;
        currentNumber = 0;
        subsPanel.SetActive(false);
        source.playOnAwake = false;
        source.loop = false;
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}
