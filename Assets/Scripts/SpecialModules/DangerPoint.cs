using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����, ���������� �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class DangerPoint : MonoBehaviour
{
    [Range(1,10), Tooltip("����, ��������� ������ � �������")] public float damage = 1;
    [Tooltip("������ �������")] public Sprite effectSprite;
}
