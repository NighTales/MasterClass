using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Передаёт один сигнал большому коичеству модулей (используется для удобства)
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class Splitter : UsingOrigin
{
    [SerializeField] private bool UseOnStart = false;

    private void Start()
    {
        if (UseOnStart)
        {
            Use();
        }
    }

    public override void ToStart()
    {
        used = false;
    }

    public override void Use()
    {
        used = true;
        UseAll();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextUsingObjects.Count; i++)
            {
                if (nextUsingObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextUsingObjects[i].transform.position);
                }
                else
                {
                    Debug.LogWarning("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
