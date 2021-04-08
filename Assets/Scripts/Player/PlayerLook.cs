using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Обзор")]
    [SerializeField, Tooltip("Объект - камера")] private Transform cam;
    [SerializeField, Tooltip("Объект - пустышка, в которой находится камера")] private Transform camBufer;
    [SerializeField, Range(1, 20), Tooltip("Чувствительность камеры по горизонтали")]
    private float sensitivityHor = 9.0f;
    [SerializeField, Range(1, 20), Tooltip("Чувствительность камеры по вертикали")]
    private float sensitivityVert = 9.0f;
    [SerializeField, Tooltip("Ограничение угла камеры снизу"), Range(-89, 0)] private float minimumVert = -45.0f;
    [SerializeField, Tooltip("Ограничение угла камеры сверху"), Range(0, 89)] private float maximumVert = 45.0f;

    private float _rotationX = 0;
    private bool opportunityToView;

    private void Start()
    {
        StartCoroutine(SetOpportunityToViewAfterDelay(0, true));
    }

    void LateUpdate()
    {
        if(opportunityToView)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        float delta = Input.GetAxis("Mouse X") * sensitivityHor;
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    public void ToMenuState(Transform lookPoint)
    {
        _rotationX = 0;
        StartCoroutine(SetOpportunityToViewAfterDelay(0, false));
        StartCoroutine(SmootMoveCamCoroutine(lookPoint.position, lookPoint.rotation));
    }
    public void ToDefaultState()
    {
        StartCoroutine(SmootMoveCamCoroutine(camBufer.position, camBufer.rotation));
        StartCoroutine(SetOpportunityToViewAfterDelay(1, true));
    }

    private IEnumerator SmootMoveCamCoroutine(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float t = 0;
        while(t < 1)
        {
            yield return null;

            t += Time.deltaTime;

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Lerp(startRot, targetRot, t);
        }

        cam.position = targetPos;
        cam.rotation = targetRot;
    }

    private IEnumerator SetOpportunityToViewAfterDelay(float delayTime, bool state)
    {
        yield return new WaitForSeconds(delayTime);

        Cursor.lockState = state? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
        opportunityToView = state;
    }
}
