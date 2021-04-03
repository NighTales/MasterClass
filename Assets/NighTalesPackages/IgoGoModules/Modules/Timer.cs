using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// когда данный модуль активируется, то запускается обратный отсчёт, после которого он передаёт сигнал следующим модулям
/// </summary>
public class Timer : UsingOrigin
{
    [Space(20), Tooltip("Время, через которое сигнал будет передан другим модулям"), Range(0, 3600)] public float time;
    [Tooltip("Запускать сразу")] public bool active;
    [Tooltip("")] public bool drawTimerInvoke;

    public event Action<string> timerChanged;
    public event Action stopTimerEvent;

    private float currentTime;
    private string lastTimerString = string.Empty;

    private void Start()
    {
        if(active)
        {
            Use();
        }
    }
    private void Update()
    {
        if(used && active)
        {
            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if(drawTimerInvoke)
                {
                    DrawTimerInvoke();
                }
            }
            else
            {
                used = true;
                active = false;
                UseAll();
                currentTime = 0;
                if (drawTimerInvoke)
                    stopTimerEvent?.Invoke();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.white;
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

    public override void Use()
    {
        active = true;
        used = true;
        currentTime = time;
    }
    public override void ToStart()
    {
        currentTime = time;
        used = false;
        currentTime = 0;
    }

    private void DrawTimerInvoke()
    {
        string currentTimerString = (int)(currentTime / 60) + ":" + (int)(currentTime % 60);
        
        if(!currentTimerString.Equals(lastTimerString))
        {
            lastTimerString = currentTimerString;
            timerChanged?.Invoke(lastTimerString);
        }
    }
}
