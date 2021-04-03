using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("Перемещение")]
    [SerializeField, Range(1, 10)] private float speed = 5f;
    [SerializeField, Range(1, 50)] private float jumpForce = 15.0f;
    [SerializeField, Range(-40, -1)] private float terminalVelocity = -10.0f;
    [SerializeField, Tooltip("Сила притяжения на земле"), Range(-2, 0)] private float minFall = -1.5f;
    [SerializeField, Range(0.1f, 20)] private float gravity = 9.8f;


    private CharacterController charController;
    private Vector3 moveVector;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;
    private bool opportunityToMove;

    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
        opportunityToMove = true;
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
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
        opportunityToMove = charController.enabled = false;
    }
    public void ReturnLocomotionOpportunity()
    {
        opportunityToMove = charController.enabled = true;
    }
    public void SmoothMoveToPoint(Transform point)
    {
        StartCoroutine(SmoothMoveToPointCoroutine(point));
    }
    private IEnumerator SmoothMoveToPointCoroutine(Transform point)
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while(t < 1)
        {
            t += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPos, point.position, t);
            transform.rotation = Quaternion.Lerp(startRot, point.rotation, t);
            yield return null;
        }
        transform.position = point.position;
        transform.rotation = point.rotation;
    }

    private void Jump()
    {
        if (charController.isGrounded)
        {
            fallTimer = 0;
            fall = true;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            if (fall)
            {
                vertSpeed -= gravity * 5 * Time.deltaTime;
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
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        charController.Move(moveVector);
    }
}
