using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerModule : InteractableItem
{
    [SerializeField]
    private Transform playerPoint;
    [SerializeField]
    private Transform playerLookPoint;
    [SerializeField]
    private Text passwordText;
    [SerializeField]
    private string password;
    [SerializeField]
    private GameObject passwordPack;
    [SerializeField]
    private GameObject commandPack;
    [SerializeField]
    private GameObject blockPack;
    [SerializeField]
    [Range(1, 10)]
    private int numberOfAttempts = 3;
    [SerializeField]
    private InputField inputField;

    private PlayerLocomotion playerLokomotion;
    private PlayerLook playerLook;
    private PlayerUI playerUI;
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
            if(numberOfAttempts == 0)
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
        col.enabled = false;
    }
    public void ToDefault()
    {
        col.enabled = true;
        playerUI.SetPointerVisible(true);
        playerLokomotion.ReturnLocomotionOpportunity();
        playerLook.ToDefaultState();
    }

    void Start()
    {
        col = GetComponent<Collider>();
        playerLokomotion = FindObjectOfType<PlayerLocomotion>();
        playerLook = FindObjectOfType<PlayerLook>();
        playerUI = FindObjectOfType<PlayerUI>();
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
