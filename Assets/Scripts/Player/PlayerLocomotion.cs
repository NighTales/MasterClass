using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("ѕеремещение")]
    [SerializeField] private Transform cam;
    [SerializeField, Range(1, 10)] private float speed = 5f;
    [SerializeField, Range(1, 50)] private float jumpForce = 15.0f;
    [SerializeField, Range(-40, -1)] private float terminalVelocity = -10.0f;
    [SerializeField, Tooltip("—ила прит€жени€ на земле"), Range(-2, 0)] private float minFall = -1.5f;
    [SerializeField, Range(0.1f, 20)] private float gravity = 9.8f;


    private CharacterController charController;
    private Vector3 moveVector;
    private Vector3 horSpeed;
    private float sprintMultiplicatorBufer;
    private float currentSprintReloadTime;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;

    private Vector3 CamRightZero => new Vector3(cam.right.x, 0, cam.right.z);
    private Vector3 CamForwardZero => new Vector3(cam.forward.x, 0, cam.forward.z);

    private void Start()
    {
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
        sprintMultiplicatorBufer = 1;
        fall = true;
    }
    void Update()
    {
        Jump();
        PlayerMove();
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
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        moveVector = CamForwardZero * deltaZ + CamRightZero * deltaX;
        //ќграничим движение по диагонали той же скоростью, что и движение параллельно ос€м
        moveVector = Vector3.ClampMagnitude(moveVector, speed) * sprintMultiplicatorBufer;
        horSpeed = moveVector;
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        //ѕреобразуем вектор движени€ от локальных к глобальным координатам.
        moveVector = transform.TransformDirection(moveVector);
        charController.Move(moveVector);
    }
}
