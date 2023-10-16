using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArrowReminder : MonoBehaviour
{
    private RectTransform rect;
    private Image image;
    private GameManager gm;
    private float _timer = 5f;
    private Vector2 curPos;
    private bool isPlaying;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        curPos = rect.anchoredPosition;
        image = GetComponent<Image>();
        gm = GameManager.Instance;
    }


    private void OnEnable()
    {
        if (Globals.CurrentLevel > 3)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.BonusProgress > 0.99f)
        {
            if (!image.enabled) image.enabled = true;

            if (_timer > 4f)
            {
                _timer = 0;
                StartCoroutine(play());
            }
            else
            {
                _timer += Time.deltaTime;

            }
        }
        else
        {
            if (image.enabled) image.enabled = false;
        }
    }

    private IEnumerator play()
    {
        rect.anchoredPosition = curPos;
        isPlaying = true;

        rect.DOAnchorPos(new Vector2(curPos.x, curPos.y + 50), 0.5f);
        yield return new WaitForSeconds(0.5f);

        rect.DOAnchorPos(new Vector2(curPos.x, curPos.y), 0.5f);
        yield return new WaitForSeconds(0.5f);

        rect.DOAnchorPos(new Vector2(curPos.x, curPos.y + 50), 0.5f);
        yield return new WaitForSeconds(0.5f);

        rect.DOAnchorPos(new Vector2(curPos.x, curPos.y), 0.5f);
        yield return new WaitForSeconds(0.5f);
        isPlaying = false;
    }
}
