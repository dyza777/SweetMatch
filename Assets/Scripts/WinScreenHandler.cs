using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private FadeInOut WinScreenFadeInOut;
    [SerializeField] private FadeInOut[] StarsList;
    [SerializeField] private GameObject WinVFX;

    public void SetupAndShowWinScreen(float timeLeft)
    {
        SetupTime(timeLeft);
        WinScreenFadeInOut.EnableAndFadeIn();
        StartCoroutine(ShowStars());
    }

    void SetupTime(float value)
    {
        int minutes = (int)(value / 60);
        int seconds = (int)(value % 60);

        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();

        if (secondsString.Length < 2)
        {
            secondsString = "0" + secondsString;
        }

        timeLeftText.text = $"{minutesString}:{secondsString}";
    }

    IEnumerator ShowStars()
    {
        foreach (FadeInOut star in StarsList)
        {
            star.gameObject.SetActive(false);
        }
        while (WinScreenFadeInOut.FadeCanvasGroup.alpha < 1) yield return null;

        var vfx = Instantiate(WinVFX, StarsList[1].gameObject.transform) ;
        Destroy(vfx, 2);

        foreach (FadeInOut star in StarsList)
        {
            star.EnableAndFadeIn();
            while (star.FadeCanvasGroup.alpha < 1) yield return null;
        }
    }
}
