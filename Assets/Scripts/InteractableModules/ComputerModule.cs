using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerModule : InteractableItem
{
    [SerializeField]
    private Transform playerPoint;
    [SerializeField]
    private Transform playerLookPoint;

    private PlayerLocomotion playerLokomotion;
    private PlayerLook playerLook;
    private Collider col;


    public override void ToStart()
    {

    }

    public override void Use()
    {
        playerLokomotion.FreezeLocomotion();
        playerLokomotion.SmoothMoveToPoint(playerPoint);
        playerLook.ToMenuState(playerLookPoint);
        col.enabled = false;
    }
    public void ToDefault()
    {
        col.enabled = true;
        playerLokomotion.ReturnLocomotionOpportunity();
        playerLook.ToDefaultState();
    }


    void Start()
    {
        col = GetComponent<Collider>();
        playerLokomotion = FindObjectOfType<PlayerLocomotion>();
        playerLook = FindObjectOfType<PlayerLook>();
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
