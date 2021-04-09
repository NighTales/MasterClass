using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Данный модуль используется для движения объекта между двумя точками
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class TransformModule : UsingObject
{
    private enum LoopType
    {
        Once,
        PingPong,
        Repeat
    }

    [SerializeField]
    [Tooltip("Тип перемещения:" +
        "Once - одно действие=одно перемещение. " +
        "loop - когда объект доходит до финиша, он телепортируется на начало. " +
        "Ping-Pong - движение туда-обратно.")]
    private LoopType loopType;

    [SerializeField, Tooltip("Сколько будет длиться перемещение (с)"), Min(0)] private float duration = 1;
    [SerializeField, Tooltip("График изменения скорости")] private AnimationCurve accelCurve;
    [SerializeField, Tooltip("Смещение целевой точки относительно стартовой")] private Vector3 end = Vector3.forward;

    [Tooltip("Использовать ли физическое перемещение. Если вы будите двигать пол, оно будет правильней просчитывать" +
        "движение объектов вместе с ним. Но тратит больше ресурсов.")]
    private bool usePhysics = false;

    private Platform platform;
    private Rigidbody rb;
    private Vector3 start;
    private Vector3 target;
    private bool activate = false;
    private float time = 0f;
    private float position = 0f;
    private float direction = 1f;

    private void Awake()
    {
        if(usePhysics)
            rb = GetComponent<Rigidbody>();
        start = transform.position;
        target = transform.position + transform.forward * end.z + transform.right * end.x + transform.up * end.y;
    }

    [ContextMenu("Test Start Movement")]
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
        var pos = Vector3.Lerp(start, target, curvePosition);
        Vector3 deltaPosition = pos - transform.position;
        if (Application.isEditor && !Application.isPlaying)
            transform.position = pos;

        if (usePhysics)
        {
            rb.MovePosition(pos);
        }
        else
        {
            transform.position = pos;
        }

        if (platform != null)
            platform.MoveCharacterController(deltaPosition);
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
    /// Once - Начать движение
    /// Loop,PingPong - Начать/Остановить движение
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
