using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ��������� ������ � ��������� ������� � �������
/// </summary>
public class GameResultModule : UsingObject
{
    [SerializeField]
    [TextArea]
    [Tooltip("����� �� ��������� ������")]
    private string text;

    [SerializeField]
    [Tooltip("������ �� ��������� ������")]
    private Sprite icon;

    private PlayerUI playerUI;

    void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {
    }

    public override void Use()
    {
        playerUI.SetFinal(icon, text);
    }
}
