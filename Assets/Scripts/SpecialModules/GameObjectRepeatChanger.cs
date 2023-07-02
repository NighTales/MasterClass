using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRepeatChanger : UsingObject
{
    [SerializeField]
    private GameObject defaultGroup;
    [SerializeField]
    private GameObject activeGroup;
    [Min(0.01f)]
    [SerializeField]
    private float toActiveDelay = 1;
    [Min(0.01f)]
    [SerializeField]
    private float toDefaultDelay = 1;

    private bool isActive;

    public override void ToStart()
    {

    }

    public override void Use()
    {
        isActive = !isActive;
        if (isActive)
        {
            StartCoroutine(RepeatActionCoroutine());
        }
        else
        {
            StopCoroutine(RepeatActionCoroutine());
            defaultGroup.SetActive(true);
            activeGroup.SetActive(false);
        }
    }

    private IEnumerator RepeatActionCoroutine()
    {
        while (true)
        {
            defaultGroup.SetActive(false);
            activeGroup.SetActive(true);

            yield return new WaitForSeconds(toDefaultDelay);

            defaultGroup.SetActive(true);
            activeGroup.SetActive(false);

            yield return new WaitForSeconds(toActiveDelay);
        }
    }
}
