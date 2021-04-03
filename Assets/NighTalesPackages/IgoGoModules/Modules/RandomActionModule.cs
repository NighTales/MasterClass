using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// даёт сигнал случайному модулю из набора
/// </summary>
public class RandomActionModule : UsingOrigin
{
    [Tooltip("Одноразовый")] public bool once;
    [Tooltip("С удалением использованного модуля")] public bool withRemoving;

    public override void ToStart()
    {
        used = false;
    }
    public override void Use()
    {
        if(!once || (once && !used))
        {
            if (nextUsingObjects != null)
            {
                int index = Random.Range(0, nextUsingObjects.Count);
                nextUsingObjects[index].Use();
                if (withRemoving)
                    nextUsingObjects.Remove(nextUsingObjects[index]);
                used = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextUsingObjects.Count; i++)
            {
                if (nextUsingObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextUsingObjects[i].transform.position);
                    Gizmos.DrawSphere(Vector3.Lerp(transform.position, nextUsingObjects[i].transform.position,
                        Vector3.Distance(nextUsingObjects[i].transform.position, transform.position) / 2), 0.3f);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
