public class ElectronoicLockPickItem : InteractableItem
{

    private PlayerInfoHolder playerInfoHolder;
    private PlayerUI playerUI;

    private void Start()
    {
        playerInfoHolder = FindObjectOfType<PlayerInfoHolder>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {

    }

    public override void Use()
    {
        playerInfoHolder.electronicLockPickItemsCount++;
        playerUI.ClearPointer();
        Destroy(gameObject);
    }
}
