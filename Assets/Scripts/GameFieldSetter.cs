using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldSetter : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    void Start()
    {
        Vector2 leftBorder = Camera.main.ViewportToWorldPoint(new Vector2(0, 0.5f));
        Vector2 rightBorder = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));

        Vector3 leftWallPosition = new Vector3(leftBorder.x, leftWall.transform.position.y, leftWall.transform.position.z);
        leftWall.transform.position = leftWallPosition;

        Vector3 rightWallPosition = new Vector3(rightBorder.x, rightWall.transform.position.y, rightWall.transform.position.z);
        rightWall.transform.position = rightWallPosition;
    }
}
