using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDrawer : MonoBehaviour
{
    public Timer mainTimer;
    public Text timerText;

    private void Start()
    {
        CloseTimer();
        mainTimer.timerChanged += DrawTimerValue;
        mainTimer.stopTimerEvent += CloseTimer;
    }

    public void DrawTimerValue(string value)
    {
        timerText.text = value;
    }
    public void CloseTimer() => timerText.text = string.Empty;
}
