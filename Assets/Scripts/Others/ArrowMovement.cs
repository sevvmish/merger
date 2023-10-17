using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _position;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _position = transform.position;

        Sequence sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Append(_transform.DOMove(_position + Vector3.up * 2, 0.5f).SetEase(Ease.OutSine));
        sequence.Append(_transform.DOMove(_position, 0.5f).SetEase(Ease.Linear));
    }        
}
