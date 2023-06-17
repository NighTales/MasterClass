using UnityEngine;

/// <summary>
/// Поворачивет объект между двумя положениями
/// </summary>
[HelpURL("https://docs.google.com/document/d/1pw27W5hjFhxL4ex7-dp54W8nYJdjPtroRF2IrF2ZLLs/edit?usp=sharing")]
public class Rotor : UsingObject
{
    [SerializeField]
    [Tooltip("Тип вращения:" +
    "Once - одно действие=одно вращение. " +
    "loop - когда объект разворачивается до конца, он телепортируется в стартовое положение. " +
    "Ping-Pong - вращение туда-обратно.")]
    private LoopType loopType;

    [SerializeField, Tooltip("Сколько будет длиться вращение (с)"), Min(0)] private float duration = 1;
    [SerializeField, Tooltip("График изменения скорости")] private AnimationCurve accelCurve;
    [SerializeField, Tooltip("Отклонение конечного поворота от начального (смотри вектор forward) в градусах")]
    private Vector3 rotVector = new Vector3(0,90,0);

    private Quaternion localStart;
    private Quaternion localTarget;
    private bool activate = false;
    private float time = 0f;
    private float angle = 0f;
    private float direction = 1f;

    private void Awake()
    {
        localStart = transform.localRotation;
        localTarget = localStart * Quaternion.Euler(rotVector);
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
            PerformRotation(angle);
        }
    }

    public override void ToStart()
    {

    }

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

    private void StartMovement()
    {
        activate = true;
    }

    private void PerformRotation(float position)
    {
        var curveAngle = accelCurve.Evaluate(position);
        var ang = Quaternion.Lerp(localStart, localTarget, curveAngle);
        transform.localRotation = ang;
    }

    void LoopPingPong()
    {
        angle = Mathf.PingPong(time, 1f);
    }

    void LoopRepeat()
    {
        angle = Mathf.Repeat(time, 1f);
    }

    void LoopOnce()
    {
        angle = Mathf.Clamp01(time);
        if (angle <= 0 || angle >= 1)
        {
            time = angle;
            activate = false;
            direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Quaternion Target = transform.localRotation * Quaternion.Euler(rotVector);
            Vector3 start, end;
            start = transform.position + transform.forward * 3;
            float t = 0;
            Gizmos.DrawLine(transform.position, start);
            Gizmos.DrawSphere(start, 0.4f);
            while (t < 1)
            {
                t += Time.fixedDeltaTime;
                Quaternion currentTarget = Quaternion.Lerp(transform.localRotation, Target, t);
                end = transform.position + (transform.localRotation * currentTarget * Vector3.forward * 3);
                Gizmos.DrawLine(start, end);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(end, end + transform.localRotation * currentTarget * Vector3.up * 1);
                start = end;
                Gizmos.color = Color.cyan;
            }
            end = transform.position + transform.localRotation * Target * Vector3.forward * 3;
            Gizmos.DrawWireSphere(end, 0.4f);
        }
    }
}
