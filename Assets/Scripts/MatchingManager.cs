using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MatchingManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> matchCellsPositions;
    [SerializeField] private List<MatchObject> matchCellsInfo = new List<MatchObject>(7);
    [SerializeField] private GameObject MatchVFX;
    private GoalsHandler goalsHandler;
    private ObjectsSpawner objectsSpawner;
    private int currentMatchStartIndex = -1;
    private List<MatchObject> matchedObjects;

    void Start()
    {
        matchedObjects = new List<MatchObject>();
        goalsHandler = FindObjectOfType<GoalsHandler>();
        objectsSpawner = FindObjectOfType<ObjectsSpawner>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            matchCellsPositions.Add(child.position + new Vector3(0, 0.4f, 0));
        }
    }

    public void OccupyNewCell(MatchObject matchObject)
    {
        if (matchCellsInfo.Count >= 7)
        {
            return;
        }

        FindObjectOfType<GoalsHandler>().ProgressInGoal(matchObject);
        matchObject.GetReadyForMatching();
        RecalculateCells(matchObject);
        MoveObjectsToPositions();
        if (matchCellsInfo.Count < 3) return;
        StartCoroutine(CheckMatch(matchObject));
    }

    void RecalculateCells(MatchObject matchObject)
    {
        int cellIndex = -1;
        for (int i = 0; i < matchCellsInfo.Count; i++)
        {
            if (matchCellsInfo[i].name == matchObject.name )
            {
                cellIndex = i + 1;
            }
        }

        if (cellIndex != -1)
        {
            RearrangeCells(cellIndex, matchObject);
        } else
        {
            matchCellsInfo.Add(matchObject);
        }
    }

    void RearrangeCells(int cellIndex, MatchObject matchObject)
    {
        matchCellsInfo.Add(matchCellsInfo[matchCellsInfo.Count - 1]);

        for (int i = matchCellsInfo.Count - 1; i > cellIndex; i--)
        {
            matchCellsInfo[i] = matchCellsInfo[i - 1];
        }

        matchCellsInfo[cellIndex] = matchObject;
    }

    void MoveObjectsToPositions()
    {
        for (int i = 0; i < matchCellsInfo.Count; i++)
        {
            matchCellsInfo[i].transform.DOMove(matchCellsPositions[i], 0.3f);
        }
    }

    IEnumerator CheckMatch(MatchObject matchObject)
    {
        yield return new WaitForSeconds(0.3f);
        bool isMatch = false;

        for (int i = 0; i <= matchCellsInfo.Count - 3; i++)
        {
            if (matchedObjects.Contains(matchCellsInfo[i])) continue;
            if (AreSameObjects(matchCellsInfo[i], matchCellsInfo[i + 1]) && AreSameObjects(matchCellsInfo[i + 1], matchCellsInfo[i + 2]))
            {
                while (matchObject != null && DOTween.IsTweening(matchObject.transform)) yield return null;
                if (matchObject == null) yield break;
                matchedObjects.Add(matchCellsInfo[i]);
                matchedObjects.Add(matchCellsInfo[i+1]);
                matchedObjects.Add(matchCellsInfo[i+2]);
                isMatch = true;
                currentMatchStartIndex = i;
                ConfirmMatch(i, new List<MatchObject>() { matchCellsInfo[i], matchCellsInfo[i+1], matchCellsInfo[i+2] });
            }
        }

        if (!isMatch && matchCellsInfo.Count >= 7 && currentMatchStartIndex == -1)
        {
            FindObjectOfType<Gameplay>().HandleFail();
        }
        yield return null;
    }

    void ConfirmMatch(int cellIndex, List<MatchObject> matchedList)
    {
        matchCellsInfo[cellIndex].transform.DOMove(matchCellsPositions[cellIndex + 1], 0.2f);
        matchCellsInfo[cellIndex + 2].transform.DOMove(matchCellsPositions[cellIndex + 1], 0.2f).OnComplete(() =>
        {
            GameObject vfx = Instantiate(MatchVFX, matchCellsPositions[cellIndex + 1], Quaternion.identity);
            Destroy(vfx, 1);

            objectsSpawner.RemoveObjectsFromPool(matchedList);

            foreach(MatchObject matchObject in matchedList)
            {
                matchedObjects.Remove(matchObject);
                Destroy(matchObject.gameObject);
                matchCellsInfo.Remove(matchObject);
            }
            
            currentMatchStartIndex = -1;
            MoveObjectsToPositions();
        });
    }

    bool AreSameObjects(MatchObject object1, MatchObject object2) => object1.name == object2.name;

    public void UseMagnet()
    {
        LevelGoal targetGoal = null;
        List<MatchObject> upcomingMatch = new List<MatchObject>();

        for (int i = 0; i < matchCellsInfo.Count; i++)
        {
            if (targetGoal != null)
            {
                if (targetGoal.GoalName != GetMatchObjectName(matchCellsInfo[i])) continue;
                upcomingMatch.Add(matchCellsInfo[i]);
            } else
            {
                targetGoal = goalsHandler.goalsObjects.FirstOrDefault<LevelGoal>(levelGoal => levelGoal.GoalName == GetMatchObjectName(matchCellsInfo[i]));
                if (targetGoal == null) continue;
                upcomingMatch.Add(matchCellsInfo[i]);
            }
        }

        if (targetGoal == null)
        {
            do
            {
                targetGoal = goalsHandler.goalsObjects[Random.Range(0, goalsHandler.goalsObjects.Count)];
            } while (targetGoal.CurrentGoalCount == 0);
        }

        int objectToAttractCount = 0;
        while (upcomingMatch.Count < 3)
        {
            var objectToAttract = objectsSpawner.objectsList.First<MatchObject>(matchObject => GetMatchObjectName(matchObject) == targetGoal.GoalName && !upcomingMatch.Contains(matchObject));
            upcomingMatch.Add(objectToAttract);
            objectToAttractCount++;
        }

        if (objectToAttractCount > (7 - matchCellsInfo.Count)) return;

        for (int i = (3 - objectToAttractCount); i < 3; i++)
        {
            OccupyNewCell(upcomingMatch[i]);
        }
    }

    string GetMatchObjectName(MatchObject matchObject) => matchObject.name.Substring(0, matchObject.name.Length - 7);
}
