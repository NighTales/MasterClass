using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualModule : UsingOrigin
{
    public bool startActionManually = false;

    public override void ToStart()
    {
    }

    public override void Use()
    {
        UseAll();
    }

    private void OnDrawGizmos()
    {
        if(startActionManually)
        {
            Use();
            startActionManually = false;
        }

        if (debug)
        {
            Gizmos.color = new Color(255, 134, 0, 255);
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
