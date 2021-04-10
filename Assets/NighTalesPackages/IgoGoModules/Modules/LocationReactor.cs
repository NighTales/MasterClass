using UnityEngine;

/// <summary>
/// Зона, при входе в котороую ModuleReactor будет запускать импульс
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class LocationReactor : UsingOrigin
{
    [Space(20)]
    [Tooltip("Реагировать только на вход")] public bool enterOnly;
    [Tooltip("Отключаться после первого срабатывания")] public bool once;

    public override void Use()
    {
        UseAll();
        used = !used;
        if (once)
        {
            gameObject.SetActive(false);
        }
    }
    public override void ToStart()
    {
        used = false;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
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
