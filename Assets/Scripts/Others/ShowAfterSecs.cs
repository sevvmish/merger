using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAfterSecs : MonoBehaviour
{
    public float delay = 0.75f;

    private void OnEnable()
    {
        StartCoroutine(play());
    }

    private IEnumerator play()
    {
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, delay));
        transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOPunchPosition(Vector3.one, 0.2f).SetEase(Ease.InOutFlash);
        SoundController.Instance.PlayUISound(SoundsUI.pop);
    }
}
