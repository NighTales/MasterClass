using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ����������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1VJXjmNxYcbPJk1MIiedqgAUKW1hjnBcMdRIIKpdvyHs/edit?usp=sharing")]
public class ComputerModule : InteractableItem
{
    [Header("���������")]
    [Tooltip("��� ������������ ���������� ��� ��� ���������")] public string login;
    [Tooltip("������ ������������")] public string password;
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
            attemtsCountText.text = "������� " + numberOfAttempts;
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
        attemtsCountText.text = "������� " + numberOfAttempts;
        loginText.text = login;
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
