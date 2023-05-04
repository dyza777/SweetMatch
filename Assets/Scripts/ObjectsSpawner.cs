using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public List<MatchObject> objectsList { get; private set; }
    [SerializeField] private GameObject OnboardingObjectsPrefab;
    public void SetUpLevel(LevelData levelData)
    {
        objectsList = new List<MatchObject>();
        Vector3 fieldSize = GetComponent<BoxCollider>().bounds.size;

        if (!StaticFields.onboardingCompleted)
        {
            GameObject onboardingObjects = Instantiate(OnboardingObjectsPrefab, Vector3.zero, Quaternion.identity);
            foreach (Transform child in onboardingObjects.transform)
            {
                objectsList.Add(child.GetComponent<MatchObject>());
            }
            FindObjectOfType<OnboardingManager>().StartOnboarding(objectsList);
        }
        else
        {
            SpawnObjects(levelData.levelGoalsInfo, fieldSize);
            SpawnObjects(levelData.junkObjectsInfo, fieldSize);
        }
    }

    void SpawnObjects(List<LevelObjectInfo> objectsInfo, Vector3 fieldSize)
    {
        for (int i = 0; i < objectsInfo.Count; i++)
        {
            for (int k = 0; k < (int)objectsInfo[i].objectCount; k++)
            {
                var position = GetRandomPosition(fieldSize);
                var rotation = GetRandomRotation();
                GameObject obj = Instantiate(objectsInfo[i].objectPrefab, position, rotation);
                objectsList.Add(obj.GetComponent<MatchObject>());
            }
        }
    }

    public void RemoveObjectsFromPool(List<MatchObject> objectsToRemove)
    {
        foreach (MatchObject objToRemove in objectsToRemove)
        {
            objectsList.Remove(objToRemove);
        }
    }

    Vector3 GetRandomPosition(Vector3 fieldSize)
    {
        var xCoord = Random.Range(transform.position.x - fieldSize.x / 2, transform.position.x + fieldSize.x / 2);
        var zCoord = Random.Range(transform.position.z - fieldSize.z / 2, transform.position.z + fieldSize.z / 2);
        var yCoord = Random.Range(transform.position.y - 0.3f, transform.position.y + 0.5f);

        return new Vector3(xCoord, yCoord, zCoord);
    }

    Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
    }
}
