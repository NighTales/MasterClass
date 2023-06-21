using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������� �����
/// </summary>
[HelpURL("https://docs.google.com/document/d/1Lx0ViX8BewkGp61OfT_7HXL8JeKo0wQ8C6-cbwad3wc/edit?usp=sharing")]
public class PasswordItemContainer : InteractableItem
{
    [SerializeField, Tooltip("����� ��� ����� ���������� ������")] private Text passwordText;
    [SerializeField, Tooltip("����� ��� ����� ���������� login")] private Text userText;
    [SerializeField, Tooltip("�� ������ ���������� ��� ������")] private ComputerModule computer;

    private PlayerInfoHolder playerInfoHolder;
    private PlayerUI playerUI;

    private void Start()
    {
        computer.profile.ProfileDataChanged += OnProfileDataChanged;
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        playerUI = FindObjectOfType<PlayerUI>();
        OnProfileDataChanged();
    }

    public override void ToStart()
    {

    }

    public void OnProfileDataChanged()
    {
        userText.text = "����� ����������\n\rLogIn: " + computer.profile.login;
        passwordText.text = computer.profile.password;
    }

    public override void Use()
    {
        playerInfoHolder.AddPasswordItem(computer);
        playerUI.ClearPointer();
        computer.profile.ProfileDataChanged -= OnProfileDataChanged;
        Destroy(gameObject);
    }
}
