using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("На какие слои не реагировать")]
    private LayerMask ignoreMask;
    [SerializeField, Range(1,3), Tooltip("На каком расстоянии можно взаимодействовать")]
    private float interactionDistance = 1;
    [SerializeField, Tooltip("Какую кнопку использовать для взаимодействия")]
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
            interactableItem?.Use();
        }
    }
}

[RequireComponent(typeof(Collider))]
public abstract class InteractableItem : UsingOrigin
{
    [Tooltip("Текстовая подсказка при наведении на объект")] public string message;
    [Tooltip("Значок прицела при наведении на объект")] public Sprite messageSprite;
}
