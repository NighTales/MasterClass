using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField, Range(1,3)] private float interactionDistance = 1;
    [SerializeField] private KeyCode interactionButton = KeyCode.E;

    private Transform myTransform;
    private PlayerUI playerUI;
    private RaycastHit hit;
    private Collider bufer;
    private ManualModule currentModule;

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
                if (hit.collider.CompareTag("ManualModule"))
                {
                    currentModule = hit.collider.GetComponent<ManualModule>();
                    bufer = hit.collider;
                    playerUI.SetMessage(interactionButton + " - " + currentModule.message, currentModule.messageSprite);
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
            playerUI.Clear();
            bufer = null;
            currentModule = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(interactionButton))
        {
            currentModule?.Use();
        }
    }
}
