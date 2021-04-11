using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��������� ��������� ������� ������
/// </summary>
public class TaskChanger : UsingObject
{
    [TextArea]
    [SerializeField]
    [Tooltip("����� ������")]
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
