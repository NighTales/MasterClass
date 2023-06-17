using UnityEngine;

/// <summary>
/// Этот сприпт вешается на игрока и позваляет ему испускать импульс при взаимодействии с триггером модуля
/// </summary>
[HelpURL("https://docs.google.com/document/d/1UomX5zFCg7eJs_tM_oHi7osBHw32YkJt4N7x3QAXOI8/edit?usp=sharing")]
public class ModuleReactor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Interactable"))
        {
            if(other.TryGetComponent(out LocationReactor loc))
            {
                loc.Use();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Interactable"))
        {
            if (other.TryGetComponent(out LocationReactor loc))
            {
                if (!loc.enterOnly)
                {
                    loc.Use();
                }
            }
        }
    }
}
