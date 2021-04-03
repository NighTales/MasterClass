using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicChanger : MonoBehaviour
{
    [SerializeField, Tooltip("Клип, который будет назначен активным после того, входа в триггер")] private AudioClip clip = null;
    [SerializeField, Tooltip("Эта зона смены музыки будет работать один раз")] private bool once = false;

    public AudioClip CheckChange()
    {
        if (once)
            Destroy(gameObject, 0.3f);

        return clip;
    }
}
