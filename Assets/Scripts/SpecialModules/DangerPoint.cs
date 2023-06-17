using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����, ���������� �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/16TuOOuhUkWtscFk0D3IT9jpPY8AifB063ohqiaJsNDs/edit?usp=sharing")]
public class DangerPoint : MonoBehaviour
{
    [Range(1,10), Tooltip("����, ��������� ������ � �������")] public float damage = 1;
    [Tooltip("������ �������")] public Sprite effectSprite;
}
