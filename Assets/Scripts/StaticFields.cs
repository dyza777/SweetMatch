using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFields
{
    public static bool onboardingCompleted = false;
    public static int CurrentLevelNumber = 1;
    public static bool[] levelsCompleted = { false, false, false, false, false };
    public static bool isInitialized = false;
}
