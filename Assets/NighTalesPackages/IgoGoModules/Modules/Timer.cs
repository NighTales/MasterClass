using System;
using UnityEngine;


/// <summary>
/// когда данный модуль активируется, то запускается обратный отсчёт, после которого он передаёт сигнал следующим модулям
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class Timer : UsingOrigin
{
    [Space(20), Tooltip("Время в секундах, через которое сигнал будет передан другим модулям"), Range(0, 3600)]
    [SerializeField]
    private float time;

    [SerializeField]
    [Tooltip("Запускать сразу")]
    private bool active;
    
    [Tooltip("Отрисовывать время")]
    [SerializeField]
    private bool useTimerUI;


    private event Action<string> timerChanged;
    private event Action stopTimerEvent;

    private float currentTime;
    private string lastTimerString = string.Empty;

    private void Start()
    {
        if(useTimerUI)
        {
            TimerUI timerUI = FindObjectOfType<TimerUI>();

            timerChanged += timerUI.DrawTimerValue;
            stopTimerEvent += timerUI.CloseTimer;
        }

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
                if(useTimerUI)
                    DrawTimerInvoke();
            }
            else
            {
                used = true;
                active = false;
                UseAll();
                currentTime = 0;
                if(useTimerUI)
                    stopTimerEvent?.Invoke();
                ToStart();
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

    /// <summary>
    /// Запустить таймер
    /// </summary>
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
