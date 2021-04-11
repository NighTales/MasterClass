using UnityEngine;

public enum LoopType
{
    Once,
    PingPong,
    Repeat
}

/// <summary>
/// ������ ������ ������������ ��� �������� ������� ����� ����� �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class TransformModule : UsingObject
{
    [SerializeField]
    [Tooltip("��� �����������:" +
        "Once - ���� ��������=���� �����������. " +
        "loop - ����� ������ ������� �� ������, �� ��������������� �� ������. " +
        "Ping-Pong - �������� ����-�������.")]
    private LoopType loopType;

    [SerializeField, Tooltip("������� ����� ������� ����������� (�)"), Min(0)] private float duration = 1;
    [SerializeField, Tooltip("������ ��������� ��������")] private AnimationCurve accelCurve;
    [SerializeField, Tooltip("�������� ������� ����� ������������ ���������")] private Vector3 end = Vector3.forward;

    private Vector3 localStart;
    private Vector3 localTarget;
    private bool activate = false;
    private float time = 0f;
    private float position = 0f;
    private float direction = 1f;

    private void Awake()
    {
        localStart = transform.localPosition;
        localTarget = transform.localPosition + end;
    }

    private void StartMovement()
    {
        activate = true;
    }

    private void Update()
    {
        if (activate)
        {
            time = time + (direction * Time.deltaTime / duration);
            switch (loopType)
            {
                case LoopType.Once:
                    LoopOnce();
                    break;
                case LoopType.PingPong:
                    LoopPingPong();
                    break;
                case LoopType.Repeat:
                    LoopRepeat();
                    break;
            }
            PerformTransform(position);
        }
    }

    private void PerformTransform(float position)
    {
        var curvePosition = accelCurve.Evaluate(position);
        var pos = Vector3.Lerp(localStart, localTarget, curvePosition);
        transform.localPosition = pos;
    }

    void LoopPingPong()
    {
        position = Mathf.PingPong(time, 1f);
    }

    void LoopRepeat()
    {
        position = Mathf.Repeat(time, 1f);
    }

    void LoopOnce()
    {
        position = Mathf.Clamp01(time);
        if (position <= 0 || position >= 1)
        {
            time = position;
            activate = false;
            direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Vector3 targetPos = transform.position + transform.forward * end.z + transform.right * end.x + transform.up * end.y;
            Gizmos.DrawWireCube(targetPos, transform.localScale);
            Gizmos.DrawLine(transform.position, targetPos);

        }
    }

    /// <summary>
    /// Once - ������ ��������
    /// Loop,PingPong - ������/���������� ��������
    /// </summary>
    public override void Use()
    {
        if (loopType == LoopType.Once)
        {
            StartMovement();
        }
        else
        {
            if (!activate)
            {
                StartMovement();
            }
            else
            {
                activate = false;
            }
        }
    }

    public override void ToStart()
    {

    }
}
