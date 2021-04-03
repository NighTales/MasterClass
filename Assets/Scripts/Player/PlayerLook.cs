using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Обзор")]
    [SerializeField] private Transform viewObject;
    [SerializeField, Range(1, 20)] private float sensitivityHor = 9.0f;
    [SerializeField, Range(1, 20)] private float sensitivityVert = 9.0f;
    [SerializeField, Tooltip("Ограничение угла камеры снизу"), Range(-89, 0)] private float minimumVert = -45.0f;
    [SerializeField, Tooltip("Ограничение угла камеры сверху"), Range(0, 89)] private float maximumVert = 45.0f;

    private float _rotationX = 0;

    void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        float delta = Input.GetAxis("Mouse X") * sensitivityHor;
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        viewObject.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }
}
