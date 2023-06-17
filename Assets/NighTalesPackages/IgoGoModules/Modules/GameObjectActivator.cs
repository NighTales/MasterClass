using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Задаёт указанным объектам значение activeSalfe, равное state
/// </summary>
[HelpURL("https://docs.google.com/document/d/1SSYvJQCSI5n47luaeOEiMVqYWpMTlGxABMUtG7VGBAQ/edit?usp=sharing")]
public class GameObjectActivator : UsingObject
{
    [Tooltip("Объекты, которые будут переключены. TargetState для каждого после переключения" +
        "сменится на противоположный. Модуль можно будет использовать повторно для обратного эффекта.")]
    [SerializeField]
    private List<StateContainer> targets;

    /// <summary>
    /// Переключить активность указанных объектов
    /// </summary>
    public override void Use()
    {
        SetStateForAll();
        used = true;
    }
    public override void ToStart()
    {
        used = false;
    }

    private void SetStateForAll()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                targets[i].targetGO.SetActive(targets[i].targetState);
                targets[i].targetState = !targets[i].targetState;
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.position, 0.3f);

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null && targets[i].targetGO != null)
                {
                    if (targets[i].targetState)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawLine(transform.position, targets[i].targetGO.transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}

[System.Serializable]
public class StateContainer
{
    [Tooltip("Объект, которому нужно задать состояние")] public GameObject targetGO;
    [Tooltip("Целевое состояние. Если отмечено, объект будет включен")] public bool targetState = false;
}
