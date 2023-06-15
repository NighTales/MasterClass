using UnityEngine;

/// <summary>
/// ������ ��������� ��������� ������� ������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class TaskChanger : UsingObject
{
    [TextArea]
    [SerializeField]
    [Tooltip("����� ������")]
    private string taskString;

    private PlayerUI playerUI;

    private void Awake()
    {
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {
    }

    public override void Use()
    {
        playerUI.SetTask(taskString);
    }
}
