using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт компьютера
/// </summary>
[HelpURL("https://docs.google.com/document/d/1VJXjmNxYcbPJk1MIiedqgAUKW1hjnBcMdRIIKpdvyHs/edit?usp=sharing")]
public class ComputerModule : InteractableItem
{
    [Header("Настройки")]
    public ProfileItem profile;
    [Range(1, 10)]
    [SerializeField]
    [Tooltip("Количество попыток перед блокировкой")]
    private int numberOfAttempts = 3;
    [SerializeField]
    [Tooltip("Точка, куда встаёт персонаж")]
    private Transform playerPoint;
    [SerializeField]
    [Tooltip("Точка, куда подлетает камера")]
    private Transform playerLookPoint;

    [Header("UI")]
    [SerializeField]
    private Text loginText;
    [SerializeField]
    private Text passwordText;
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
            commandPack.SetActive(true);
            passwordPack.SetActive(false);
        }
        else
        {
            currentAttemptsCount--;
            attemtsCountText.text = "Попыток " + currentAttemptsCount;
            if (currentAttemptsCount == 0)
            {
                blockPack.SetActive(true);
                passwordPack.SetActive(false);
            }
        }
    }

    public override void Use()
    {
        playerLokomotion.FreezeLocomotion();
        playerLokomotion.SmoothMoveToPoint(playerPoint);
        playerLook.ToMenuState(playerLookPoint);
        playerUI.SetPointerVisible(false);
        playerInfoHolder.FindPassword(this);
        col.enabled = false;
    }
    public void ToDefault()
    {
        col.enabled = true;
        playerUI.SetPointerVisible(true);
        playerLokomotion.ReturnLocomotionOpportunity();
        playerLook.ToDefaultState();
        playerUI.ClearPassword();
    }

    public void OnProfileDataChanged()
    {
        blockPack.SetActive(false);
        passwordPack.SetActive(true);
        loginText.text = profile.login;
        currentAttemptsCount = numberOfAttempts;
        attemtsCountText.text = "Попыток " + currentAttemptsCount;
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
