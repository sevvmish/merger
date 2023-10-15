using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamePush;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Infromer panel")]
    [SerializeField] private Image[] informForFutureImages;
    [SerializeField] private GameObject informerPanel;
    [SerializeField] private Transform framePointer;
    [SerializeField] private RectTransform informerImage1;
    private Vector2 informerImage1Base;

    [Header("Level messaging")]
    [SerializeField] private GameObject messagingPanel;
    [SerializeField] private TextMeshProUGUI mainTexterText;
    [SerializeField] private RectTransform mainTextRect;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;


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
    [SerializeField] private Sprite eventReplaceSprite;
    [SerializeField] private Sprite blanck;

    [Header("Houses sprites")]
    [SerializeField] private Button bonusButton;
    [SerializeField] private Image bonusProgressImage;
    [SerializeField] private GameObject bonusFVX;

    [SerializeField] private Interstitial interstitial;

    [Header("bonus texter")]
    [SerializeField] private GameObject bonusTextExample;
    [SerializeField] private Transform locationaForBonusText;
    private ObjectPool bonusPlusDataPool;
    private bool isShowLeft;

    private Queue<GameEventsType> gameEventsPack = new Queue<GameEventsType>();
    private GameManager gm;
    private bool isInitial;
    private bool isChangingInProgress;
    private Translation lang;
    

    private void Start()
    {
        gm = GameManager.Instance;
        bonusPlusDataPool = new ObjectPool(5, bonusTextExample, locationaForBonusText);

        Off();

        
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        informerImage1 = informForFutureImages[0].GetComponent<RectTransform>();
        informerImage1Base = informerImage1.sizeDelta;

        bonusButton.onClick.AddListener(() => 
        {
            if (gm.BonusProgress < 0.99f)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return;
            }

            SoundController.Instance.PlayUISound(SoundsUI.click);
            gm.SpendBonusActivated();
        });

        continueButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            Off();
            if (Globals.CurrentLevel > 1 
            && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERSTITIAL_COOLDOWN)
            {
                interstitial.OnEnded = GameManager.Instance.StartSimpleGame;
                interstitial.ShowInterstitialVideo();
            }
            else
            {
                GameManager.Instance.StartSimpleGame();
            }
            
        });

        restartButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            Off();
            if (Globals.CurrentLevel > 1
            && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERSTITIAL_COOLDOWN)
            {
                interstitial.OnEnded = GameManager.Instance.StartSimpleGame;
                interstitial.ShowInterstitialVideo();
            }
            else
            {
                GameManager.Instance.StartSimpleGame();
            }
        });
    }

    public void SetMessagingLevel(bool isON)
    {
        if (isON)
        {
            messagingPanel.SetActive(true);

            if (Globals.CurrentLevel == 0)
            {
                mainTexterText.text = lang.TutorialText;
            }
            else
            {
                mainTexterText.text = lang.LevelText + " " + Globals.CurrentLevel;
            }

            
            mainTextRect.localScale = Vector3.zero;
            mainTextRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutElastic);
        }
        else
        {
            Off();
        }
        
    }

    public void SetMessagingWin()
    {
        SoundController.Instance.PlayUISound(SoundsUI.win);
        messagingPanel.SetActive(true);
        mainTexterText.text = lang.WinText;
        StartCoroutine(playWin());
    }
    private IEnumerator playWin()
    {
        mainTextRect.localScale = Vector3.zero;
        mainTextRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        mainTextRect.DOShakeScale(0.3f, 0.1f, 30).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        yield return new WaitForSeconds(0.5f);

        mainTextRect.DOAnchorPos(new Vector2(0,100), 0.3f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        continueButton.gameObject.SetActive(true);

    }

    public void SetMessagingLose()
    {
        SoundController.Instance.PlayUISound(SoundsUI.lose);
        messagingPanel.SetActive(true);
        mainTexterText.text = lang.LoseText;
        StartCoroutine(playLose());

    }
    private IEnumerator playLose()
    {
        mainTextRect.localScale = Vector3.zero;
        mainTextRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        mainTextRect.DOShakeScale(0.3f, 0.1f, 30).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        yield return new WaitForSeconds(0.5f);

        mainTextRect.DOAnchorPos(new Vector2(0,100), 0.3f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.31f);

        restartButton.gameObject.SetActive(true);
    }

    public void ShowBonusAddedText(int amount)
    {
        
        StartCoroutine(playShowBonus(amount));
    }
    private IEnumerator playShowBonus(int amount)
    {
        if (isShowLeft)
        {
            yield return new WaitForSeconds(0);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }
        

        RectTransform rect = bonusPlusDataPool.GetObject().GetComponent<RectTransform>();
        rect.gameObject.SetActive(true);
        rect.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one * 0.2f;
        Vector2 endPos = Vector2.zero;
        if (isShowLeft)
        {
            isShowLeft = false;
            endPos = new Vector2(UnityEngine.Random.Range(180, 200), UnityEngine.Random.Range(110, 130));
        }
        else
        {
            isShowLeft = true;
            endPos = new Vector2(UnityEngine.Random.Range(110, 130), UnityEngine.Random.Range(180, 200));
        }
            

        rect.DOAnchorPos(endPos, 1.5f).SetEase(Ease.OutQuad);
        rect.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(1.5f);
        rect.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);

        bonusPlusDataPool.ReturnObject(rect.gameObject);
    }

    public void Off()
    {
        mainTextRect.DOAnchorPos(Vector2.zero, 0);
        informerPanel.SetActive(false);
        bonusButton.gameObject.SetActive(false);
        bonusFVX.SetActive(false);
        scorePanel.SetActive(false);
        messagingPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
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
            if (Globals.IsPlayingSimpleGame && !Globals.IsPlayingCustomGame)
            {
                scoreText.text = (gm.ScoreProgress * 100).ToString("f0") + "%";
            }
            else if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
            {
                scoreText.text = gm.Score.ToString();
            }
                                    
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

            case GameEventsType.replace_house:
                return eventReplaceSprite;
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
