using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����, ���������� �������
/// </summary>
public class DangerPoint : MonoBehaviour
{
    [Range(1,10), Tooltip("����, ��������� ������ � �������")] public float damage = 1;
    [Tooltip("������ �������")] public Sprite effectSprite;
}
