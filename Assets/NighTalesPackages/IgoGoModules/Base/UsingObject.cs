using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// базовый модуль, который может выполнять действия
/// </summary>
public abstract class UsingObject: MonoBehaviour
{
    [Tooltip("Добавить вспомогательную отрисовку")] public bool debug;

    public List<UsingOrigin> Actors
    {
        get
        {
            if (_actors == null)
                _actors = new List<UsingOrigin>();

            return _actors;
        }
    }
    private List<UsingOrigin> _actors;
    [HideInInspector] public bool used;
    /// <summary>
    /// выполнить действие
    /// </summary>
    public abstract void Use();
    /// <summary>
    /// перевести в исходное состояние
    /// </summary>
    public abstract void ToStart();
    /// <summary>
    /// Уделить этот модуль из всех источникв, с ним работающих
    /// </summary>
    public void ClearMeFromActors()
    {
        foreach (var item in Actors)
        {
            item.nextUsingObjects.Remove(this);
        }
    }
}

/// <summary>
/// модуль, который может передавать сигнал о действии другим модулям
/// </summary>
public abstract class UsingOrigin : UsingObject
{
    [Tooltip("Объекты, у которых будет вызываться метод USE()")]
    public List<UsingObject> nextUsingObjects;
    
    private void Awake()
    {
        AddThisActorToNextModules();
    }

    /// <summary>
    /// передать сигнал следующим модулям
    /// </summary>
    /// 
    public void UseAll()
    {
        for (int i = 0; i < nextUsingObjects.Count; i++)
        {
            if (nextUsingObjects[i] != null)
            {
                nextUsingObjects[i].Use();
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }

    private void AddThisActorToNextModules()
    {
        foreach (var item in nextUsingObjects)
        {
            item.Actors.Add(this);
        }
    }
}
