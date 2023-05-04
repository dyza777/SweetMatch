using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointerSwaying : MonoBehaviour
{
    void Start()
    {
        float initZ = transform.position.z;
       transform.DOMoveZ(initZ + 0.4f, 0.4f).SetLoops(-1, LoopType.Yoyo);
    }
}
