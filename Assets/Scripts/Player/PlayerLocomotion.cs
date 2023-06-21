using System.Collections;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Скрипт перемещения
/// </summary>
[HelpURL("https://docs.google.com/document/d/139dOQl7Xa8lBEhb_pupl78IZYufyxDxKlUC4A7kfCE4/edit?usp=sharing")]
public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float speed = 5f;
    [SerializeField, Range(1, 50), Tooltip("Сила прыжка")] private float jumpForce = 15.0f;
    [SerializeField, Range(1, 5), Tooltip("Скорость перехода к движению в приседе")] private float sitStateChangeSpeed = 2;
    [SerializeField, Range(-40, -1)]
    [Tooltip("Ограничение скорости падения. Это требуется, чтобы персонаж," +
        "падающий с большой высоты не проникал сквозь текстуры.")]
    private float terminalVelocity = -10.0f;
    [SerializeField, Range(0.1f, 5), Tooltip("Сила притяжения. g=1 - земная гравитация")] private float gravity = 1f;

    [SerializeField] private Transform jumpCheck;
    [SerializeField] private LayerMask ignoreMask;

    [SerializeField] private Transform faceHeight;
    [SerializeField] private Transform stayFacePoint;
    [SerializeField] private Transform sitFacePoint;

    private Vector3 moveVector;
    private CapsuleCollider capsuleCollider;
    private float vertSpeed;
    private JumpState jumpState;
    private bool opportunityToMove;
    private float minFall = -1.5f;
    private Transform oldParent;

    private SitState sitState = SitState.Stay;

    /// <summary>
    /// Этот коэффициент используется, чтобы добиться ощущения "правильной" гравитации при gravity = 1.
    /// </summary>
    private const float gravMultiplayer = 9.8f * 5f;

    private Transform myTransform;
    private Collider transformFixator;

    private void Start()
    {
        myTransform = transform;
        opportunityToMove = true;
        vertSpeed = minFall;
        jumpState = JumpState.Stay;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if(opportunityToMove)
        {
            Jump();
            PlayerMove();
            SitStateToggle();
        }
    }

    public void FreezeLocomotion()
    {
        opportunityToMove = false;
    }
    public void ReturnLocomotionOpportunity()
    {
        opportunityToMove = true;
    }
    public void SmoothMoveToPoint(Transform point)
    {
        StartCoroutine(SmoothMoveToPointCoroutine(point));
    }

    public void SetBlockValueToPlayer(bool value)
    {
        opportunityToMove = !value;
    }
    public void TeleportToPoint(Transform point)
    {
        myTransform.position = point.position;
        myTransform.rotation = point.rotation;
    }

    private void Jump()
    {
        if (IsGrounded() && jumpState == JumpState.Stay)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce * (sitState == SitState.Stay? 1 : 0.5f);
                jumpState = JumpState.Jump;
                return;
            }
            vertSpeed = 0;
        }
        else
        {
            vertSpeed -= gravity * gravMultiplayer * Time.deltaTime;

            if (vertSpeed <= 0)
            {
                jumpState = JumpState.Fall;
            }

            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }
    }
    private void PlayerMove()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed * (sitState == SitState.Stay? 1 : 0.5f);
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        myTransform.position += moveVector;
    }
    private void SitStateToggle()
    {
        if(Input.GetButtonDown("Sit") && opportunityToMove && sitState != SitState.ChangeState)
        {
            if (sitState == SitState.Stay)
            {
                sitState = SitState.ChangeState;
                StartCoroutine(ToSitStateCoroutine());
            }
            else
            {
                sitState = SitState.ChangeState;
                StartCoroutine(ToStayStateCoroutine());
            }
        }
    }

    private bool IsGrounded()
    {
        if(jumpState == JumpState.Jump)
        { 
            return false;
        }

        Collider[] bufer = Physics.OverlapBox(jumpCheck.position, jumpCheck.localScale, Quaternion.identity, ~ignoreMask);

        if (bufer != null && bufer.Length > 0)
        {
            jumpState = JumpState.Stay;
            return true;
        }
        else
        {
            jumpState= JumpState.Fall;
            return false;
        }
    }

    private IEnumerator SmoothMoveToPointCoroutine(Transform point)
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPos, point.position, t);
            transform.rotation = Quaternion.Lerp(startRot, point.rotation, t);
            yield return null;
        }
        transform.position = point.position;
        transform.rotation = point.rotation;
    }

    private IEnumerator ToSitStateCoroutine()
    {
        float startCapsuleHeight = capsuleCollider.height;
        float targetCapsuleHeight = capsuleCollider.height / 2;
        Vector3 startCapsuleCenter = capsuleCollider.center;
        Vector3 targetCapsuleCenter = new Vector3(0, 0.52f, 0);
        Vector3 startFacePosition = faceHeight.position;
        Vector3 targetFacePosition = sitFacePoint.position;

        float t = 0;

        while (t<=1)
        {
            capsuleCollider.height = Mathf.Lerp(startCapsuleHeight, targetCapsuleHeight, t);
            capsuleCollider.center = Vector3.Lerp(startCapsuleCenter, targetCapsuleCenter, t);
            faceHeight.position = Vector3.Lerp(startFacePosition, targetFacePosition, t);
            t += Time.deltaTime * sitStateChangeSpeed;

            yield return null;
        }

        sitState = SitState.Sit;
    }
    private IEnumerator ToStayStateCoroutine()
    {
        float startCapsuleHeight = capsuleCollider.height;
        float targetCapsuleHeight = capsuleCollider.height * 2;
        Vector3 startCapsuleCenter = capsuleCollider.center;
        Vector3 targetCapsuleCenter = new Vector3(0, 0.94f, 0);
        Vector3 startFacePosition = faceHeight.position;
        Vector3 targetFacePosition = stayFacePoint.position;

        float t = 0;

        while (t <= 1)
        {
            capsuleCollider.height = Mathf.Lerp(startCapsuleHeight, targetCapsuleHeight, t);
            capsuleCollider.center = Vector3.Lerp(startCapsuleCenter, targetCapsuleCenter, t);
            faceHeight.position = Vector3.Lerp(startFacePosition, targetFacePosition, t);
            t += Time.deltaTime * sitStateChangeSpeed;

            yield return null;
        }

        sitState = SitState.Stay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TransformFixator"))
        {
            if (!transform.parent.CompareTag("TransformFixator"))
            {
                oldParent = transform.parent;
            }
            transformFixator = other;
            myTransform.parent = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other == transformFixator)
        {
            myTransform.parent = oldParent;
        }
    }


    enum SitState
    {
        Stay,
        Sit,
        ChangeState
    }

    enum JumpState
    {
        Stay,
        Jump,
        Fall
    }
}


