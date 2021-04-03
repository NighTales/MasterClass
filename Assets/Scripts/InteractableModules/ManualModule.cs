using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ManualModule : InteractableItem
{
    public override void Use()
    {
        UseAll();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = new Color(255, 134, 0, 255);
            Gizmos.DrawSphere(transform.position, 0.1f);
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

    public override void ToStart()
    {

    }
}
