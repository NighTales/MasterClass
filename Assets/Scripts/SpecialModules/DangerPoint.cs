using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Точка, отнимающая энергию
/// </summary>
[HelpURL("https://docs.google.com/document/d/16TuOOuhUkWtscFk0D3IT9jpPY8AifB063ohqiaJsNDs/edit?usp=sharing")]
public class DangerPoint : MonoBehaviour
{
    [Range(1,10), Tooltip("Урон, наносимый точкой в секунду")] public float damage = 1;
    [Tooltip("Иконка эффекта")] public Sprite effectSprite;
}
