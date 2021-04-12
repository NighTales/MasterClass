using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¬ыводит финальную панель с указанным текстом и иконкой
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class GameResultModule : UsingObject
{
    [SerializeField]
    [TextArea]
    [Tooltip("“екст на финальной панели")]
    private string text;

    [SerializeField]
    [Tooltip("»конка на финальной панели")]
    private Sprite icon;

    private PlayerUI playerUI;

    void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {
    }

    public override void Use()
    {
        playerUI.SetFinal(icon, text);
    }
}
