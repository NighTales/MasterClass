using UnityEngine;

public class ReplicModule : UsingObject
{
    private ReplicSystem system;
    [SerializeField]
    private ReplicaPack pack;

    private void Start()
    {
        system = FindObjectOfType<ReplicSystem>();
    }

    public override void ToStart()
    {
    }

    public override void Use()
    {
        system.AddNewReplicaPack(pack);
        Destroy(gameObject, Time.deltaTime);
    }
}
