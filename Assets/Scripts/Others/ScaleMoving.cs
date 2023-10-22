using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMoving : MonoBehaviour
{
    private Transform _transform;
 
    private void OnEnable()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(play());
    }

    private IEnumerator play()
    {
        while (true)
        {
            _transform.localScale = Vector3.one;
            _transform.DOScale(Vector3.one * 0.6f, 0.5f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.7f);
        }



    }
}
