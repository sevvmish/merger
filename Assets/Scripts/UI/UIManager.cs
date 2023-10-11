using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Infromer panel")]
    [SerializeField] private Image[] informForFutureImages;
    [SerializeField] private GameObject informerPanel;
    [SerializeField] private Transform framePointer;
    [SerializeField] private RectTransform informerImage1;
    private Vector2 informerImage1Base;

    [Header("Score")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int lastScore;

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
    [SerializeField] private Sprite blanck;

    [Header("Houses sprites")]
    [SerializeField] private Button bonusButton;
    [SerializeField] private Image bonusProgressImage;
    [SerializeField] private GameObject bonusFVX;

    private Queue<GameEventsType> gameEventsPack = new Queue<GameEventsType>();
    private GameManager gm;
    private bool isInitial;
    private bool isChangingInProgress;

    private void Start()
    {
        gm = GameManager.Instance;
        informerPanel.SetActive(false);
        bonusButton.gameObject.SetActive(false);
        bonusFVX.SetActive(false);
        scorePanel.SetActive(false);

        informerImage1 = informForFutureImages[0].GetComponent<RectTransform>();
        informerImage1Base = informerImage1.sizeDelta;

        bonusButton.onClick.AddListener(() => 
        {
            if (gm.BonusProgress < 0.99f) return;
                        
            gm.SpendBonusActivated();
        });
    }

    public void SetData(Queue<GameEventsType> gameEventsPack)
    {
        
        informerPanel.SetActive(true);
        bonusButton.gameObject.SetActive(true);
        bonusProgressImage.fillAmount = 0;
        this.gameEventsPack = gameEventsPack;
        gm.OnEventUpdated += eventUpdated;
        isInitial = false;
        isChangingInProgress = false;
        scorePanel.SetActive(true);
        scoreText.text = gm.Score.ToString();
        lastScore = gm.Score;

        StartCoroutine(playShake(framePointer));
        eventUpdated();
    }

    private void eventUpdated()
    {
        StartCoroutine(playEventUpdate());
    }
    private IEnumerator playEventUpdate()
    {
        while (isChangingInProgress)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isChangingInProgress = true;
        yield return new WaitForSeconds(0);

        if (isInitial)
        {            
            informForFutureImages[0].sprite = blanck;
            for (int i = 1; i < 15; i++)
            {
                informerImage1.sizeDelta = new Vector2(informerImage1Base.x, informerImage1Base.y / i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        isInitial = true;
        informerImage1.sizeDelta = informerImage1Base;

        int index = 0;
        foreach (var item in gameEventsPack)
        {
            if (index < informForFutureImages.Length)
            {
                informForFutureImages[index].sprite = getSpriteByGameEvent(item);
                index++;
            }
        }

        isChangingInProgress = false;
    }

    private void Update()
    {        
        if (gm.IsGameStarted && lastScore != gm.Score)
        {
            scoreText.text = gm.Score.ToString();
            lastScore = gm.Score;
        }

        if (gm.IsGameStarted)
        {
            bonusProgressImage.fillAmount = gm.BonusProgress;

            if (gm.BonusProgress >= 0.99f && !bonusFVX.activeSelf)
            {
                bonusFVX.SetActive(true);
                bonusButton.transform.DOShakeScale(0.75f, 0.5f, 30).SetEase(Ease.InOutElastic);
                //SoundController.Instance.PlayUISound(SoundsUI.positive);
            }
            else if (gm.BonusProgress < 0.99f && bonusFVX.activeSelf)
            {
                bonusFVX.SetActive(false);
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
