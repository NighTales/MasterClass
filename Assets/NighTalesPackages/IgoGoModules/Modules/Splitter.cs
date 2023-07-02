using UnityEngine;

/// <summary>
/// Передаёт один сигнал большому коичеству модулей (используется для удобства)
/// </summary>
[HelpURL("https://docs.google.com/document/d/1uuKl2mXgrk8ZtGrdyVY8JK23MDIb2eEX-_ulNJs1WVo/edit?usp=sharing")]
public class Splitter : UsingOrigin
{
    [SerializeField, Tooltip("Запустить сразу")] private bool UseOnStart;

    private void Start()
    {
        if (UseOnStart)
        {
            Use();
        }
    }

    public override void ToStart()
    {
        used = false;
    }

    public override void Use()
    {
        used = true;
        UseAll();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
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
