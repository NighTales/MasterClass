using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Зона, при входе в котороую ModuleReactor будет запускать импульс
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class LocationReactor : UsingOrigin
{
    [Space(20)]
    [Tooltip("Реагировать только на вход")] public bool enterOnly;
    [Tooltip("Уничтожаться после первого срабатывания")] public bool once;

    public override void Use()
    {
        UseAll();
        if(once)
        {
            gameObject.SetActive(false);
            used = true;
        }
    }
    public override void ToStart()
    {
        used = false;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextUsingObjects.Count; i++)
            {
                if (nextUsingObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextUsingObjects[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
