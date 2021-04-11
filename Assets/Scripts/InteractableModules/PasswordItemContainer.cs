using UnityEngine;
using UnityEngine.UI;

public class PasswordItemContainer : InteractableItem
{
    [SerializeField] private Text passwordText;
    [SerializeField] private Text userText;
    [SerializeField] private ComputerModule computer;

    private PlayerInfoHolder playerInfoHolder;
    private PlayerUI playerUI;

    private void Start()
    {
        userText.text = "Карта сотрудника\n\rLogIn: " + computer.login;
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
