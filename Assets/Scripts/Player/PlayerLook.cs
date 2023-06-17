using System.Collections;
using UnityEngine;

/// <summary>
/// ������ ������ �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1dm9wemLIta_WcqR-Kz0fanKv5ICR9wrwQAKpvAr_468/edit?usp=sharing")]
public class PlayerLook : MonoBehaviour
{
    [SerializeField, Tooltip("������ - ������")] private Transform cam;
    [SerializeField, Tooltip("������ - ��������, � ������� ��������� ������")] private Transform camBufer;
    [SerializeField, Range(0, 10), Tooltip("���������������� ������ �� �����������")]
    private float sensitivityHor = 0.5f;
    [SerializeField, Range(0, 10), Tooltip("���������������� ������ �� ���������")]
    private float sensitivityVert = 0.5f;
    [SerializeField, Tooltip("����������� ���� ������ �����"), Range(-90, 0)] private float minimumVert = -45.0f;
    [SerializeField, Tooltip("����������� ���� ������ ������"), Range(0, 90)] private float maximumVert = 45.0f;

    private float _rotationX = 0;
    private bool opportunityToView;
    private const float multiplicator = 100;

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
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * Time.deltaTime * multiplicator;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        float delta = Input.GetAxis("Mouse X") * sensitivityHor * Time.deltaTime * multiplicator;
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    public void SetCursorVisible(bool value)
    {
        if(value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(opportunityToView)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void ToMenuState(Transform lookPoint)
    {
        _rotationX = 0;
        StartCoroutine(SetOpportunityToViewAfterDelay(0, false));
        StartCoroutine(SmootMoveCamCoroutine(lookPoint));
    }
    public void ToDefaultState()
    {
        StartCoroutine(SmootMoveCamCoroutine(camBufer));
        StartCoroutine(SetOpportunityToViewAfterDelay(1, true));
    }

    private IEnumerator SmootMoveCamCoroutine(Transform target)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float t = 0;
        while(t < 1)
        {
            yield return null;

            t += Time.deltaTime;

            cam.position = Vector3.Lerp(startPos, target.position, t);
            cam.rotation = Quaternion.Lerp(startRot, target.rotation, t);
        }

        yield return null;

        cam.position = target.position;
        cam.rotation = target.rotation;
    }

    private IEnumerator SetOpportunityToViewAfterDelay(float delayTime, bool state)
    {
        yield return new WaitForSeconds(delayTime);
        opportunityToView = state;
        SetCursorVisible(!state);
    }
}
