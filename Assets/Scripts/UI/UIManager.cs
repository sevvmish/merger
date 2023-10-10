using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Infromer panel")]
    [SerializeField] private Image[] informForFutureImages;
    [SerializeField] private GameObject informerPanel;
    [SerializeField] private Transform framePointer;

    [Header("Houses sprites")]
    [SerializeField] private Sprite eventOneSprite;
    [SerializeField] private Sprite eventTwoSprite;
    [SerializeField] private Sprite eventThreeSprite;
    [SerializeField] private Sprite eventFourSprite;
    [SerializeField] private Sprite eventFiveSprite;
    [SerializeField] private Sprite eventSixSprite;
    [SerializeField] private Sprite eventSevenSprite;
    [SerializeField] private Sprite eventDeleteSprite;
    [SerializeField] private Sprite eventUpSprite;

    [Header("Houses sprites")]
    [SerializeField] private Button bonusButton;
    [SerializeField] private Image bonusProgressImage;
    [SerializeField] private GameObject bonusFVX;

    private Queue<GameEventsType> gameEventsPack = new Queue<GameEventsType>();
    private GameManager gm;

    private void Start()
    {
        informerPanel.SetActive(false);
        bonusButton.gameObject.SetActive(false);
        bonusFVX.SetActive(false);
    }

    public void SetData(Queue<GameEventsType> gameEventsPack)
    {
        gm = GameManager.Instance;
        informerPanel.SetActive(true);
        bonusButton.gameObject.SetActive(true);
        bonusProgressImage.fillAmount = 0;
        this.gameEventsPack = gameEventsPack;
        gm.OnEventUpdated += eventUpdated;

        StartCoroutine(playShake(framePointer));
        eventUpdated();
    }

    private void eventUpdated()
    {
        int index = 0;
        foreach (var item in gameEventsPack)
        {
            if (index < informForFutureImages.Length)
            {
                informForFutureImages[index].sprite = getSpriteByGameEvent(item);
                index++;
            }
        }
    }

    private Sprite getSpriteByGameEvent(GameEventsType _type) 
    {
        switch(_type)
        {
            case GameEventsType.house_one:
                return eventOneSprite;

            case GameEventsType.house_two:
                return eventTwoSprite;

            case GameEventsType.house_three:
                return eventThreeSprite;

            case GameEventsType.house_four:
                return eventFourSprite;

            case GameEventsType.house_five:
                return eventFiveSprite;

            case GameEventsType.house_six:
                return eventSixSprite;

            case GameEventsType.house_seven:
                return eventSevenSprite;

            case GameEventsType.up_house:
                return eventUpSprite;

            case GameEventsType.delete_house:
                return eventDeleteSprite;
        }

        return null;
    }

    private IEnumerator playShake(Transform _transform)
    {
        while (true)
        {
            _transform.DOShakeScale(0.3f, 0.1f, 30).SetEase(Ease.InOutElastic);
            yield return new WaitForSeconds(1f);
        }
    }
}
