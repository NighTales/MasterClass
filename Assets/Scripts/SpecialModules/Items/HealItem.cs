public class HealItem : InteractableItem
{
    private PlayerEnergyControl playerEnegyControl;
    private PlayerUI playerUI;

    private void Start()
    {
        playerEnegyControl = FindObjectOfType<PlayerEnergyControl>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public override void ToStart()
    {

    }

    public override void Use()
    {
        playerEnegyControl.AddHealItem();
        playerUI.ClearPointer();
        Destroy(gameObject);
    }
}
