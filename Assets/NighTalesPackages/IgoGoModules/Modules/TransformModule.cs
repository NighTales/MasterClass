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
[HelpURL("https://docs.google.com/document/d/1pw27W5hjFhxL4ex7-dp54W8nYJdjPtroRF2IrF2ZLLs/edit?usp=sharing")]
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

    private Vector3 Start;
    private Vector3 Target;
    private bool activate = false;
    private float time = 0f;
    private float position = 0f;
    private float direction = 1f;

    private void Awake()
    {
        Start = transform.position;
        Target = transform.TransformPoint(end);
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
        var pos = Vector3.Lerp(Start, Target, curvePosition);
        transform.position = pos;
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
            Vector3 targetPos = transform.TransformPoint(end);
            Gizmos.DrawWireCube(targetPos, transform.localScale);
            Gizmos.DrawLine(transform.position, targetPos);

        }
    }

    /// <summary>
    /// Once - ������ ��������
    /// Loop,PingPong - ������/���������� ��������
    /// </summary>
    [ContextMenu("Use")]
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
