using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class ComputerModule : InteractableItem
{
    [Header("Настройки")]
    public string login;
    public string password;
    [Range(1, 10)]
    [SerializeField]
    private int numberOfAttempts = 3;
    [SerializeField]
    private Transform playerPoint;
    [SerializeField]
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
        if(passwordText.text.Equals(password))
        {
            commandPack.SetActive(true);
            passwordPack.SetActive(false);
        }
        else
        {
            numberOfAttempts--;
            attemtsCountText.text = "Попыток " + numberOfAttempts;
            if (numberOfAttempts == 0)
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

    void Start()
    {
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        col = GetComponent<Collider>();
        playerLokomotion = FindObjectOfType<PlayerLocomotion>();
        playerLook = FindObjectOfType<PlayerLook>();
        playerUI = FindObjectOfType<PlayerUI>();
        attemtsCountText.text = "Попыток " + numberOfAttempts;
        loginText.text = "Добро пожаловать, " + login;
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
