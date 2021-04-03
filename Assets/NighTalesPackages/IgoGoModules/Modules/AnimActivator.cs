﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Этот модуль позволяет менять значение у аниматоров, которые имеют только два состояния Active/Disactive.
/// Хорошо подходит для особых дверей, ламп, и других объектов, имеющих два состояния,
/// которые нельзя реализовать другими модулями. Также есть возможность плавно изменять состояние анимации по
/// шкале типа float. 
/// 
/// У аниматоров должны быть параметры: Active(для boll) или Value (для использования float)
/// </summary>
public class AnimActivator : UsingObject {

    [Tooltip("Аниматор должен содержать параметр Active (bool)")]
    public List<Animator> animObjects;
    [SerializeField]public bool useFloat;
    [SerializeField, Range(0.01f, 1)] public float speed = 0.1f;
    [Tooltip("Начальное состояние")] public bool active;

    private bool currentActive;
    [HideInInspector]public float target;
    private float currentValue;
    private bool change;

    private void Start()
    {
        ToStart();
    }
    private void Update()
    {
        if(change)
        {
            currentValue += Time.deltaTime * speed;
            if(currentValue > target)
            {
                currentValue = target;
                change = false;
            }
            SetActiveForAll(currentValue);
        }
    }

    public override void Use()
    {
        if(useFloat)
        {
            target += speed;
            change = true;
        }
        else
        {
            currentActive = !currentActive;
            SetActiveForAll(currentActive);
        }
        used = !used;
    }
    public override void ToStart()
    {
        used = false;
        if(useFloat)
        {
            currentValue = 0;
            SetActiveForAll(currentValue);
        }
        else
        {
            currentActive = active;
            SetActiveForAll(active);
        }
    }
    public void SetActiveForAll(bool value)
    {
        for (int i = 0; i < animObjects.Count; i++)
        {
            if(animObjects[i] != null)
            {
                animObjects[i].SetBool("Active", value);
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }
    public void SetActiveForAll(float value)
    {
        for (int i = 0; i < animObjects.Count; i++)
        {
            if (animObjects[i] != null)
            {
                animObjects[i].SetFloat("Value", value, Time.deltaTime, Time.deltaTime);
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < animObjects.Count; i++)
            {
                if (animObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, animObjects[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
