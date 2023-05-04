using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MatchObject : MonoBehaviour
{
    public Sprite objectIcon;
    private Rigidbody _rb;
    private MeshCollider _boxCollider;

    [SerializeField] private Vector3 _rotationInCell = Vector3.zero;
    [SerializeField] private Vector3 _scaleInCell = Vector3.one;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<MeshCollider>();
    }

    public void GetReadyForMatching()
    {
        _rb.isKinematic = true;
        _boxCollider.enabled = false;

        transform.DORotate(_rotationInCell, 0.3f);
        transform.localScale = _scaleInCell;
    }
}
