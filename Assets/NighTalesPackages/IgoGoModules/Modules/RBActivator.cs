﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// активирует UseGravity у указанных Rigidbody
/// </summary>
public class RBActivator : UsingObject
{
    [Tooltip("Объекты, которые должны бдуту упасть по команде.")] public List<Rigidbody> rigidbodies;

    private void Start()
    {
        if(!used)
        {
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                if (rigidbodies[i] != null)
                {
                    rigidbodies[i].isKinematic = true;
                    rigidbodies[i].useGravity = false;
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                if (rigidbodies[i] != null)
                {
                    Gizmos.DrawLine(transform.position, rigidbodies[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }

    public override void Use()
    {
        foreach (var item in rigidbodies)
        {
            if(item != null)
            {
                item.isKinematic = false;
                item.useGravity = true;
                used = true;
            }
        }
    }
    public override void ToStart()
    {
        foreach (var item in rigidbodies)
        {
            item.useGravity = false;
            item.isKinematic = true;
        }
        used = false;
    }

}
