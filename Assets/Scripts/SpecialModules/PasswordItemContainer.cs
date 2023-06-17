using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Парольная карта
/// </summary>
[HelpURL("https://docs.google.com/document/d/1Lx0ViX8BewkGp61OfT_7HXL8JeKo0wQ8C6-cbwad3wc/edit?usp=sharing")]
public class PasswordItemContainer : InteractableItem
{
    [SerializeField, Tooltip("текст где будет выводиться пароль")] private Text passwordText;
    [SerializeField, Tooltip("текст где будет выводиться login")] private Text userText;
    [SerializeField, Tooltip("от какого компьютера эти данные")] private ComputerModule computer;

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
