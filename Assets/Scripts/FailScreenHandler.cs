using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailScreenHandler : MonoBehaviour
{
    [SerializeField] private FadeInOut FailScreenFadeInOut;
    public void SetupAndShowFailScreen()
    {
        FailScreenFadeInOut.EnableAndFadeIn();
    }
}
