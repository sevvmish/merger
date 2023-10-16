using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAfterSecs : MonoBehaviour
{
    public float delay = 3f;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.pop);
            this.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        SoundController.Instance.PlayUISound(SoundsUI.positive);
        StartCoroutine(play());
    }
    private IEnumerator play()
    {
        yield return new WaitForSeconds(delay);
        SoundController.Instance.PlayUISound(SoundsUI.pop);
        this.gameObject.SetActive(false);
    }
}
