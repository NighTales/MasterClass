using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������� �����
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class PasswordItemContainer : InteractableItem
{
    [SerializeField, Tooltip("����� ��� ����� ���������� ������")] private Text passwordText;
    [SerializeField, Tooltip("����� ��� ����� ���������� login")] private Text userText;
    [SerializeField, Tooltip("�� ������ ���������� ��� ������")] private ComputerModule computer;

    private PlayerInfoHolder playerInfoHolder;
    private PlayerUI playerUI;

    private void Start()
    {
        userText.text = "����� ����������\n\rLogIn: " + computer.login;
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        playerUI = FindObjectOfType<PlayerUI>();
        passwordText.text = computer.password;
    }

    public override void ToStart()
    {

    }

    public override void Use()
    {
        playerInfoHolder.AddPasswordItem(computer);
        playerUI.ClearPointer();
        Destroy(gameObject);
    }
}
