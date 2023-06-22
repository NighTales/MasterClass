using System;
using System.Collections;
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
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip defaultLoginSound;
    [SerializeField]
    private AudioClip lockPickSound;
    [SerializeField]
    private AudioClip blockSound;


    [Header("UI")]
    [SerializeField]
    private Text loginText;
    [SerializeField]
    private Text passwordText;
    [SerializeField, Tooltip("Панель с найденным паролем")] 
    private GameObject foundedPasswordPanel;
    [SerializeField, Tooltip("Текст где прописывается найденный пароль")]
    private Text foundedPasswordText;

    [SerializeField]
    private GameObject passwordPack;
    [SerializeField]
    private GameObject commandPack;
    [SerializeField]
    private GameObject blockPack;
    [SerializeField]
    private GameObject LockPickPack;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text attemtsCountText;
    [SerializeField]
    private Text lockPickCountText;

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
            attemtsCountText.text = "Попыток " + currentAttemptsCount;
            if (currentAttemptsCount == 0)
            {
                blockPack.SetActive(true);
                passwordPack.SetActive(false);
                source.PlayOneShot(blockSound);
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
        else
        {
            ShowLockPickPanel();
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
        LockPickPack.SetActive(false);
    }

    public void OnProfileDataChanged()
    {
        blockPack.SetActive(false);
        passwordPack.SetActive(true);
        loginText.text = profile.login;
        currentAttemptsCount = numberOfAttempts;
        attemtsCountText.text = "Попыток " + currentAttemptsCount;
    }

    public void UseLockPick()
    {
        source.PlayOneShot(lockPickSound);
        playerInfoHolder.electronicLockPickItemsCount--;
        ShowMainScreen();
    }

    public void UseFoundedPassword()
    {
        source.PlayOneShot(defaultLoginSound);
        ShowMainScreen();
    }

    private void ShowMainScreen()
    {
        commandPack.SetActive(true);
        passwordPack.SetActive(false);
        blockPack.SetActive(false);
        LockPickPack.SetActive(false);
    }

    void Start()
    {
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        col = GetComponent<Collider>();
        playerLokomotion = FindObjectOfType<PlayerLocomotion>();
        playerLook = FindObjectOfType<PlayerLook>();
        playerUI = FindObjectOfType<PlayerUI>();
        loginText.text = profile.login;
        currentAttemptsCount = numberOfAttempts;
        attemtsCountText.text = "Попыток " + currentAttemptsCount;
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

    private void ShowLockPickPanel()
    {
        if(playerInfoHolder.electronicLockPickItemsCount > 0)
        {
            LockPickPack.SetActive(true);
            lockPickCountText.text = "Отмычек: " + playerInfoHolder.electronicLockPickItemsCount.ToString();
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
