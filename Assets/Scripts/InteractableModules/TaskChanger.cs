using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модуль позволяет обновлять текущую задачу
/// </summary>
public class TaskChanger : UsingObject
{
    [TextArea]
    [SerializeField]
    [Tooltip("Текст задачи")]
    private string taskString;

    private PlayerUI playerUI;

    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {
    }

    public override void Use()
    {
        playerUI.SetTask(taskString);
    }
}
