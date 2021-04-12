using UnityEngine;

/// <summary>
/// Позволяет передавать сигнал о действии объектам с помощью PlayerInteraction
/// </summary>
[RequireComponent(typeof(Collider))]
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class ManualModule : InteractableItem
{
    /// <summary>
    /// Передать сигнал о действии следующим модулям
    /// </summary>
    public override void Use()
    {
        used = !used;
        UseAll();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = new Color(255, 134, 0, 255);
            Gizmos.DrawSphere(transform.position, 0.1f);
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

    public override void ToStart()
    {
        used = false;
    }
}
