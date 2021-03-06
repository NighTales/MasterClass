using UnityEngine;
using System;

/// <summary>
/// ?????? ?????????????? ? ?????????????? ?????????
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class PlayerInteraction : MonoBehaviour
{
    public event Action spendEnergyToInteractEvent;

    [SerializeField, Tooltip("?? ????? ???? ?? ???????????")]
    private LayerMask ignoreMask;
    [SerializeField, Range(1,5), Tooltip("?? ????? ?????????? ????? ?????????????????")]
    private float interactionDistance = 3;
    [SerializeField, Tooltip("????? ?????? ???????????? ??? ??????????????")]
    private KeyCode interactionButton = KeyCode.E;

    private Transform myTransform;
    private PlayerUI playerUI;
    private RaycastHit hit;
    private Collider bufer;
    private InteractableItem interactableItem;

    private void Start()
    {
        myTransform = transform;
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void FixedUpdate()
    {
        if(Physics.Raycast(myTransform.position, myTransform.forward, out hit, interactionDistance, ~ignoreMask))
        {
            if(hit.collider != bufer)
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    interactableItem = hit.collider.GetComponent<InteractableItem>();
                    bufer = hit.collider;
                    playerUI.SetMessage(interactionButton + " - " + interactableItem.message, interactableItem.messageSprite);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        if (bufer != null)
        {
            playerUI.ClearPointer();
            bufer = null;
            interactableItem = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(interactionButton))
        {
            if(interactableItem != null)
            {
                interactableItem?.Use();
                spendEnergyToInteractEvent?.Invoke();
            }
        }
    }
}

[RequireComponent(typeof(Collider))]
public abstract class InteractableItem : UsingOrigin
{
    [Tooltip("????????? ????????? ??? ????????? ?? ??????")] public string message;
    [Tooltip("?????? ??????? ??? ????????? ?? ??????")] public Sprite messageSprite;
}
