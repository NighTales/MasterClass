using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Скрипт перемещения
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class PlayerLocomotion : MonoBehaviour
{
    public event Action spendEnergyToMoveEvent;
    public event Action spendEnergyToJumpEvent;

    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float speed = 5f;
    [SerializeField, Range(1, 50), Tooltip("Сила прыжка")] private float jumpForce = 15.0f;
    [SerializeField, Range(-40, -1)]
    [Tooltip("Ограничение скорости падения. Это требуется, чтобы персонаж," +
        "падающий с большой высоты не проникал сквозь текстуры.")]
    private float terminalVelocity = -10.0f;
    [SerializeField, Range(0.1f, 5), Tooltip("Сила притяжения. g=1 - земная гравитация")] private float gravity = 1f;

    [SerializeField] private Transform jumpCheck;
    [SerializeField] private LayerMask ignoreMask;

    private Vector3 moveVector;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;
    private bool opportunityToMove;
    private float minFall = -1.5f;

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
        fall = true;
    }
    void Update()
    {
        if(opportunityToMove)
        {
            Jump();
            PlayerMove();
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
        if (IsGrounded())
        {
            fallTimer = 0;
            fall = true;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = 0;
            }
        }
        else
        {
            if (fall)
            {
                vertSpeed -= gravity * gravMultiplayer * Time.deltaTime;
                if (vertSpeed < terminalVelocity)
                {
                    vertSpeed = terminalVelocity;
                }
            }
            else
            {
                fallTimer -= Time.deltaTime;
                if (fallTimer <= 0)
                {
                    fallTimer = 0;
                    fall = true;
                }
                vertSpeed = 0;
            }
        }
    }
    private void PlayerMove()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        myTransform.position += moveVector;
    }

    private bool IsGrounded()
    {
        Collider[] bufer = Physics.OverlapBox(jumpCheck.position, jumpCheck.localScale, Quaternion.identity, ~ignoreMask);

        if (bufer != null && bufer.Length > 0)
            return true;
        else
            return false;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TransformFixator"))
        {
            transformFixator = other;
            myTransform.parent = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other == transformFixator)
        {
            myTransform.parent = null;
        }
    }
}
