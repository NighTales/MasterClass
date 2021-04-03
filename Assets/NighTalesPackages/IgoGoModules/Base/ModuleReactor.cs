using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Этот сприпт вешается на игрока и позваляет ему испускать импульс при взаимодействии с триггером модуля
/// </summary>
public class ModuleReactor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Module"))
        {
            UsingObject usingObject = other.gameObject.GetComponent<UsingObject>();
            try
            {
                usingObject.Use();
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Module"))
        {
            UsingObject usingObject = other.gameObject.GetComponent<UsingObject>();
            try
            {
                if (usingObject is LocationReactor)
                {
                    if (!((LocationReactor)usingObject).enterOnly)
                    {
                        usingObject.Use();
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
    }
}
