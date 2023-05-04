using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] private Image GoalIcon;
    [SerializeField] private GameObject goalCompletedTick;
    [SerializeField] private TextMeshProUGUI goalCountText;
    public int CurrentGoalCount { get; private set; }
    public string GoalName { get; private set; }
    private bool isActive = false;

    public void InitGoal(LevelObjectInfo levelInfo)
    {
        GoalIcon.sprite = levelInfo.objectPrefab.GetComponent<MatchObject>().objectIcon;
        CurrentGoalCount = (int)levelInfo.objectCount;
        GoalName = levelInfo.objectPrefab.name;
        isActive = true;
    }

    void Update()
    {
        if (!isActive) return;

        if (CurrentGoalCount < 1)
        {
            goalCountText.gameObject.SetActive(false);
            goalCompletedTick.SetActive(true);
            isActive = false;
            return;
        }

        goalCountText.text = CurrentGoalCount.ToString();
    }

    public void HandleProgress()
    {
        CurrentGoalCount--;
    }
}
