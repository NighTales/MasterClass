using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ��������� ������ � ��������� ������� � �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1ZQ7WCUZ5dxR-qaPXSSIiBUKbshHIU4aBklKKmlZ1p9o/edit?usp=sharing")]
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
