using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TransformModule : UsingObject
{
    private enum LoopType
    {
        Once,
        PingPong,
        Repeat
    }

    [SerializeField] private LoopType loopType;

    [SerializeField] private float duration = 1;
    [SerializeField] private AnimationCurve accelCurve;

    [SerializeField] private bool activate = false;
    [SerializeField] private UnityEvent OnStartCommand, OnStopCommand;

    [SerializeField] private AudioClip onStartAudioclip, onEndAudioclip;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Platform platform;
    [SerializeField] private Vector3 end = Vector3.forward;
    [SerializeField] private bool usePhysics = false;


    private Rigidbody rb;
    private Vector3 start;
    float time = 0f;
    float position = 0f;
    float direction = 1f;

    private void Awake()
    {
        if(usePhysics)
            rb = GetComponent<Rigidbody>();
        start = transform.position;
    }

    [ContextMenu("Test Start Movement")]
    private void StartMovement()
    {
        activate = true;
        OnStartCommand?.Invoke();
        PlayAudioStart();
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
        var pos = Vector3.Lerp(start, start + end, curvePosition);
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
            OnStopCommand?.Invoke();
            PlayAudioEnd();
            direction *= -1;
        }
    }

    [ContextMenu("Test Start Audio")]
    private void PlayAudioStart()
    {
        if (onStartAudioclip != null)
            audioSource?.PlayOneShot(onStartAudioclip);
    }
    [ContextMenu("Test End Audio")]
    private void PlayAudioEnd()
    {
        if (onEndAudioclip != null)
            audioSource?.PlayOneShot(onEndAudioclip);
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Vector3 targetPos = transform.position + end;
            Gizmos.DrawWireCube(targetPos, transform.localScale);
            Gizmos.DrawLine(transform.position, targetPos);

        }
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

    public override void ToStart()
    {

    }
}
