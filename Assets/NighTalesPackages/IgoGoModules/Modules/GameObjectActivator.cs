using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Задаёт указанным объектам значение activeSalfe, равное state
/// </summary>
public class GameObjectActivator : UsingObject
{
    [Tooltip("Объекты, которые будут переключены")] public List<StateContainer> targets;
   
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
    public GameObject targetGO;
    public bool targetState = false;
}
