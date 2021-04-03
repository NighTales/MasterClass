using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс используется, когда нужно, чтобы все actors сработали перед тем, как сигнал пошёл дальше. Одноразовый модуль!
/// </summary>
public class Bottleneck : UsingOrigin {

    public override void Use()
    {
        Invoke("CheckAllActors", Time.deltaTime * 2);       
    }

    private void CheckAllActors()
    {
        for (int i = 0; i < Actors.Count; i++)
        {
            if (Actors[i].used)
            {
                Actors[i].nextUsingObjects.Remove(this);
                Actors.Remove(Actors[i]);
                i--;
            }
        }
        if (Actors.Count == 0)
        {
            UseAll();
            used = true;
            Destroy(gameObject);
        }
    }


    public override void ToStart()
    {

    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
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

