using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    public List<LevelObjectInfo> levelGoalsInfo;
    public List<LevelObjectInfo> junkObjectsInfo;
    [Min(5)] public int levelTime;
}

[Serializable]
public class LevelObjectInfo
{
    [Min(1)]
    public LevelGoalsCount objectCount;
    public GameObject objectPrefab;
}

public enum LevelGoalsCount
{ 
    None =  0,
    Three = 3,
    Six = 6,
    Nine = 9,
    Twelve = 12,
    Fifteen = 15,
    Eighteen = 18,
    TwentyOne = 21,
    TwentyFour = 24,
    TwentySeven = 27,
    Thirty = 30
}
