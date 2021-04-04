using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordItemContainer : InteractableItem
{
    [SerializeField] private Text text;
    [SerializeField] private ComputerModule computer;

    private PlayerInfoHolder playerInfoHolder;
    private PlayerUI playerUI;

    private void Start()
    {
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        playerUI = FindObjectOfType<PlayerUI>();
        text.text = computer.password;
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
