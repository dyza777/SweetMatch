using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private FadeInOut FirstHint;
    [SerializeField] private FadeInOut SecondHint;
    [SerializeField] private GameObject HandPointerPrefab;
    List<MatchObject> objectsList;
    private int objectsCompleted = 0;
    
    public void StartOnboarding(List<MatchObject> matchObjects)
    {
        objectsList = matchObjects;
        StartCoroutine(OnboardingCoroutine());
    }

    IEnumerator OnboardingCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        objectsList[0].GetComponent<MeshCollider>().enabled = true;
        Vector3 pointerOffset = new Vector3(0, 3, -2);
        GameObject pointer = Instantiate(HandPointerPrefab, objectsList[0].transform.position + pointerOffset, Quaternion.Euler(90,0,0));
        FirstHint.EnableAndFadeIn();

        while (objectsCompleted < 1) yield return null;
        pointer.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        pointer.transform.position = objectsList[1].transform.position + pointerOffset;
        pointer.SetActive(true);

        while (objectsCompleted < 2) yield return null;
        pointer.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        pointer.transform.position = objectsList[2].transform.position + pointerOffset;
        pointer.SetActive(true);

        while (objectsCompleted < 3) yield return null;
        pointer.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        FirstHint.FadeOutAndDisable();

        yield return new WaitForSeconds(0.3f);
        SecondHint.EnableAndFadeIn();

        while (objectsCompleted < 4) yield return null;
        SecondHint.FadeOutAndDisable();

        while (objectsCompleted < 9) yield return null;
        yield return new WaitForSeconds(0.3f);
        StaticFields.onboardingCompleted = true;
        DataManager gameData = new DataManager();
        gameData.Load();
        gameData.Data.onboardingCompleted = true;
        gameData.Data.levelsCompleted[0] = true;
        StaticFields.levelsCompleted[0] = true;
        gameData.Save();
    }

    private void Update()
    {
        TouchCheck();
    }

    void TouchCheck()
    {
        if (StaticFields.onboardingCompleted) return;
        Vector2 clickedPos = Vector2.zero;
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            clickedPos = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            clickedPos = Input.mousePosition;
        }
        else
        {
            return;
        }

        Ray raycast = Camera.main.ScreenPointToRay(clickedPos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("MatchObject"))
            {
                Debug.Log(objectsCompleted);
                ProcessPress(raycastHit.collider.gameObject);
            }
        }
    }

    void ProcessPress(GameObject pressedObject)
    {
        FindObjectOfType<MatchingManager>().OccupyNewCell(pressedObject.GetComponent<MatchObject>());
        objectsCompleted++;
        if (objectsCompleted == 1)
        {
            objectsList[1].GetComponent<MeshCollider>().enabled = true;
        } else if (objectsCompleted == 2)
        {
            objectsList[2].GetComponent<MeshCollider>().enabled = true;
        } else if (objectsCompleted == 3)
        {
            for (int i = 3; i < objectsList.Count; i++)
            {
                objectsList[i].GetComponent<MeshCollider>().enabled = true;
            }
        }
    }
}
