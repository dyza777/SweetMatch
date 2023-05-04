using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    [SerializeField] private bool shouldFadeInOnEnable;
    [SerializeField] private bool shouldFadeOutInstantly;
    [SerializeField] private float fadeDuration = 0.2f;

    [HideInInspector]
    public bool isFading = false;
    public CanvasGroup FadeCanvasGroup { get; private set; }

    void Start()
    {
        FadeCanvasGroup = GetComponent<CanvasGroup>();
        if (shouldFadeOutInstantly)
        {
            StartCoroutine(FadeOutAndDisableCoroutine(0.3f));
        } else if (shouldFadeInOnEnable)
        {
            StartCoroutine(FadeIn());
        }
    }
    public void EnableAndFadeIn()
    {
        FadeCanvasGroup = GetComponent<CanvasGroup>();
        FadeCanvasGroup.alpha = 0;
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void FadeOutAndDisable()
    {
        StartCoroutine(FadeOutAndDisableCoroutine());
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            FadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        isFading = false;
    }

    IEnumerator FadeOutAndDisableCoroutine(float delayBeforeFading = 0)
    {
        isFading = true;
        yield return new WaitForSeconds(delayBeforeFading);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            FadeCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        isFading = false;
        gameObject.SetActive(false);
    }
}
