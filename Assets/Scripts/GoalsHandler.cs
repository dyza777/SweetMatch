using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalsHandler : MonoBehaviour
{
    public List<LevelGoal> goalsObjects { get; private set; }
    [SerializeField] private GameObject levelGoalPrefab;
    private Gameplay gameplay;

    public void SetupGoals(List<LevelObjectInfo> goalsInfo)
    {
        gameplay = FindObjectOfType<Gameplay>();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        goalsObjects = new List<LevelGoal>();
        foreach (LevelObjectInfo levelInfo in goalsInfo)
        {
            GameObject goalObject = Instantiate(levelGoalPrefab, transform);
            LevelGoal levelGoal = goalObject.GetComponent<LevelGoal>();
            levelGoal.InitGoal(levelInfo);
            goalsObjects.Add(levelGoal);
        }
    }

    public void ProgressInGoal(MatchObject matchObject)
    {
        foreach (LevelGoal levelGoal in goalsObjects)
        {
            if (levelGoal.GoalName == matchObject.name.Substring(0, matchObject.name.Length - 7))
            {
                levelGoal.HandleProgress();
                CheckForWin();
                break;
            }
        }
    }

    void CheckForWin()
    {
        bool isWin = true;

        foreach (LevelGoal levelGoal in goalsObjects)
        {
            if (levelGoal.CurrentGoalCount > 0)
            {
                isWin = false;
            }
        }

        if (isWin)
        {
            StartCoroutine(HandleWin());
        }
    }

    IEnumerator HandleWin()
    {
        yield return new WaitForSeconds(0.4f);
        gameplay.HandleWin();
    }
}
