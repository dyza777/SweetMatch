using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerHandler : MonoBehaviour
{
    private bool isTimerOn = false;
    [SerializeField] private TextMeshProUGUI timerText;
    public float TimerValue { get; private set; }

    public void StartCountdown(int initialValue)
    {
        TimerValue = initialValue;
        isTimerOn = true;
    }

    void Update()
    {
        if (isTimerOn)
        {
            TimerValue -= Time.deltaTime;
            timerText.text = GetTimerString(TimerValue);

            if (TimerValue < 1)
            {
                FindObjectOfType<Gameplay>().HandleFail();
            }
        }
    }

    string GetTimerString(float value)
    {
        int minutes = (int)(value / 60);
        int seconds = (int)(value % 60);

        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();

        if (secondsString.Length < 2)
        {
            secondsString = "0" + secondsString;
        }

        return $"{minutesString}:{secondsString}";
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }
}
