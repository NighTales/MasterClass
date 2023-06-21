using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ����������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1VJXjmNxYcbPJk1MIiedqgAUKW1hjnBcMdRIIKpdvyHs/edit?usp=sharing")]
public class ComputerModule : InteractableItem
{
    [Header("���������")]
    public ProfileItem profile;
    [Range(1, 10)]
    [SerializeField]
    [Tooltip("���������� ������� ����� �����������")]
    private int numberOfAttempts = 3;
    [SerializeField]
    [Tooltip("�����, ���� ����� ��������")]
    private Transform playerPoint;
    [SerializeField]
    [Tooltip("�����, ���� ��������� ������")]
    private Transform playerLookPoint;

    [Header("UI")]
    [SerializeField]
    private Text loginText;
    [SerializeField]
    private Text passwordText;
    [SerializeField, Tooltip("������ � ��������� �������")] 
    private GameObject foundedPasswordPanel;
    [SerializeField, Tooltip("����� ��� ������������� ��������� ������")]
    private Text foundedPasswordText;

    [SerializeField]
    private GameObject passwordPack;
    [SerializeField]
    private GameObject commandPack;
    [SerializeField]
    private GameObject blockPack;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text attemtsCountText;

    private int currentAttemptsCount;
    private PlayerLocomotion playerLokomotion;
    private PlayerLook playerLook;
    private PlayerUI playerUI;
    private PlayerInfoHolder playerInfoHolder;
    private Collider col;

    public override void ToStart()
    {

    }

    public void CheckPassword()
    {
        if(passwordText.text.Equals(profile.password))
        {
            UseFoundedPassword();
        }
        else
        {
            currentAttemptsCount--;
            attemtsCountText.text = "������� " + currentAttemptsCount;
            if (currentAttemptsCount == 0)
            {
                blockPack.SetActive(true);
                passwordPack.SetActive(false);
            }
        }
    }

    public override void Use()
    {
        StartCoroutine(CheckInputCoroutine());
        playerLokomotion.FreezeLocomotion();
        playerLokomotion.SmoothMoveToPoint(playerPoint);
        playerLook.ToMenuState(playerLookPoint);
        playerUI.SetPointerVisible(false);
        if(playerInfoHolder.FindPassword(this))
        {
            foundedPasswordPanel.SetActive(true);
            foundedPasswordText.text = profile.password;
        }
        col.enabled = false;
    }
    public void ToDefault()
    {
        StopCoroutine(CheckInputCoroutine());
        col.enabled = true;
        playerUI.SetPointerVisible(true);
        playerLokomotion.ReturnLocomotionOpportunity();
        playerLook.ToDefaultState();
    }

    public void OnProfileDataChanged()
    {
        blockPack.SetActive(false);
        passwordPack.SetActive(true);
        loginText.text = profile.login;
        currentAttemptsCount = numberOfAttempts;
        attemtsCountText.text = "������� " + currentAttemptsCount;
    }

    public void UseFoundedPassword()
    {
        commandPack.SetActive(true);
        passwordPack.SetActive(false);
    }

    void Start()
    {
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        col = GetComponent<Collider>();
        playerLokomotion = FindObjectOfType<PlayerLocomotion>();
        playerLook = FindObjectOfType<PlayerLook>();
        playerUI = FindObjectOfType<PlayerUI>();
        OnProfileDataChanged();
        profile.ProfileDataChanged += OnProfileDataChanged;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(playerPoint.position, 0.3f);
            Gizmos.DrawSphere(playerLookPoint.position, 0.3f);
            Gizmos.DrawLine(playerPoint.position, playerLookPoint.position);
        }
    }

    private IEnumerator CheckInputCoroutine()
    {
        while(true) 
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                CheckPassword();
            }
            yield return null;
        }
    }
}

[System.Serializable]
public class ProfileItem
{
    public string login;
    public string password;

    public event Action ProfileDataChanged;

    public void InvokeChangeData(string newLogin, string newPassword)
    {
        login = newLogin;
        password = newPassword;

        ProfileDataChanged?.Invoke();
    }
}
