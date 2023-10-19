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
    private bool isFrameShaking;

    [Header("Level messaging")]
    [SerializeField] private GameObject messagingPanel;
    [SerializeField] private TextMeshProUGUI mainTexterText;
    [SerializeField] private RectTransform mainTextRect;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI skipButtonText;


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
    [SerializeField] private Sprite eventRandSmallSprite;
    [SerializeField] private Sprite eventRandBigSprite;
    [SerializeField] private Sprite blanck;

    [Header("Houses sprites")]
    [SerializeField] private Button bonusButton;
    [SerializeField] private Image bonusProgressImage;
    [SerializeField] private GameObject bonusFVX;

    [SerializeField] private Interstitial interstitial;
    [SerializeField] private Rewarded rewarded;

    [Header("bonus texter")]
    [SerializeField] private GameObject bonusTextExample;
    [SerializeField] private Transform locationaForBonusText;
    private ObjectPool bonusPlusDataPool;
    private bool isShowLeft;
    [SerializeField] private GameObject bonusesPanel;
    [SerializeField] private Button[] bonusesButtons;
    [SerializeField] private GameObject[] bonusesButtonsRewardedIcons;
    [SerializeField] private HorizontalLayoutGroup bonusesButtonsHorLayout;
    private int buttonBonusNumber;

    private Queue<GameEventsType> gameEventsPack = new Queue<GameEventsType>();
    private Queue<GameEventsType> bonusEventsPack = new Queue<GameEventsType>();
    private GameManager gm;
    private bool isInitial;
    private bool isChangingInProgress;
    private bool isBonusTaken;
    private HashSet<int> buttonsForReward = new HashSet<int>();

    [SerializeField] private GameObject arrowExample;
    private GameObject arrow;
    private bool isArrow;

    private void Start()
    {
        gm = GameManager.Instance;
        bonusPlusDataPool = new ObjectPool(5, bonusTextExample, locationaForBonusText);

        Off();

        if (arrow == null) arrow = Instantiate(arrowExample);
        arrow.SetActive(false);

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

            if (bonusesPanel.activeSelf)
            {
                bonusesPanel.SetActive(false);
                return;
            }

            bonusesPanel.SetActive(true);

            GameEventsType[] pack = gm.GetCurrentBonuses;
            isBonusTaken = false;

            if (pack.Length <= 2)
            {
                buttonsForReward.Clear();
            }
            else if (pack.Length <= 3)
            {
                buttonsForReward.Add(2);
            }
            else if (pack.Length <= 4)
            {
                buttonsForReward.Add(2);
                buttonsForReward.Add(3);
            }
            else if (pack.Length <= 5)
            {                
                buttonsForReward.Add(3);
                buttonsForReward.Add(4);
            }

            if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds < Globals.REWARDED_COOLDOWN)
            {
                buttonsForReward.Clear();
            }
                        
            for (int i = 0; i < bonusesButtons.Length; i++)
            {
                if (i < pack.Length)
                {
                    bonusesButtons[i].gameObject.SetActive(true);
                    bonusesButtons[i].GetComponent<Image>().sprite = getSpriteByGameEvent(pack[i]);
                    if (buttonsForReward.Contains(i))
                    {
                        bonusesButtonsRewardedIcons[i].SetActive(true);
                    }
                    else
                    {
                        bonusesButtonsRewardedIcons[i].SetActive(false);
                    }
                }
                else
                {
                    bonusesButtonsRewardedIcons[i].SetActive(false);
                    bonusesButtons[i].gameObject.SetActive(false);
                }
            }
        });

        //===============BONUSES========================
        bonusesButtons[0].onClick.AddListener(() =>
        {
            handleButton(0);
        });
        bonusesButtons[1].onClick.AddListener(() =>
        {
            handleButton(1);
        });
        bonusesButtons[2].onClick.AddListener(() =>
        {
            handleButton(2);
        });
        bonusesButtons[3].onClick.AddListener(() =>
        {
            handleButton(3);
        });
        bonusesButtons[4].onClick.AddListener(() =>
        {
            handleButton(4);
        });
        //==============================================

        continueButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            Off();
#if UNITY_EDITOR
            GameManager.Instance.StartSimpleGame();
#elif UNITY_WEBGL
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
#endif

        });

        restartButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            Off();
#if UNITY_EDITOR
            GameManager.Instance.StartSimpleGame();
#elif UNITY_WEBGL
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
#endif

        });

        skipButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            rewarded.OnError = GameManager.Instance.StartSimpleGame;
            rewarded.OnRewardedEndedOK = skipAndContinue;
            rewarded.ShowRewardedVideo();
        });
    }

    private void handleButton(int buttonNumber)
    {
        if (isBonusTaken) return;
        isBonusTaken = true;
        buttonBonusNumber = buttonNumber;

        

        if (bonusesButtonsRewardedIcons[buttonNumber].activeSelf)
        {
            rewarded.OnError = () => { isBonusTaken = false; };
            rewarded.OnRewardedEndedOK = agreeButtonBonus;
            rewarded.ShowRewardedVideo();
        }
        else
        {
            agreeButtonBonus();
        }        
    }

    private void agreeButtonBonus()
    {
        gm.SpendBonusActivated();
        showBonusButtonEffect(bonusesButtons[buttonBonusNumber]);
        addBonus(gm.GetCurrentBonuses[buttonBonusNumber]);

        if (Globals.CurrentLevel > 0 && Globals.CurrentLevel <= 3 && !isArrow)
        {
            isArrow = true;
            
            arrow.SetActive(true);
            
            for (int i = 0; i < gm.GetBaseFrames.Count; i++)
            {
                if (!gm.GetBaseFrames[i].IsEmpty() && (gm.GetBaseFrames[i].FrameType == FrameTypes.one || gm.GetBaseFrames[i].FrameType == FrameTypes.two))
                {
                    
                    //arrow.transform.position = gm.GetBaseFrames[i].gameObject.transform.position + Vector3.up * 1.5f;
                    arrow.transform.position = new Vector3(gm.GetBaseFrames[i].Location.x, 1.5f, gm.GetBaseFrames[i].Location.y);
                    //print(i + ": " + gm.GetBaseFrames[i].IsEmpty() + gm.GetBaseFrames[i].Location + " = " + arrow.transform.position);
                    break;
                }
            }
        }
    }

    private void addBonus(GameEventsType bonus)
    {
        SoundController.Instance.PlayUISound(SoundsUI.positive);
        gm.AddBonus(bonus);

        int index = 0;
        foreach (var item in gm.GetCurrentEventPack)
        {
            if (index < informForFutureImages.Length)
            {
                informForFutureImages[index].sprite = getSpriteByGameEvent(item);
                index++;
            }
        }

    }

    private void showBonusButtonEffect(Button button)
    {
        StartCoroutine(playBonusButtonEffect(button.GetComponent<RectTransform>()));
    }
    private IEnumerator playBonusButtonEffect(RectTransform rect)
    {
        bonusesButtonsHorLayout.enabled = false;
        Vector2 oldPos = rect.anchoredPosition;
        rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 250), 0.4f).SetEase(Ease.InOutQuad);
        rect.DOScale(Vector3.one * 2.5f, 0.4f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(0.4f);

        rect.GetComponent<Image>().DOFade(0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        rect.anchoredPosition = oldPos;
        rect.localScale = Vector3.one;
        rect.GetComponent<Image>().DOFade(1, 0);
        bonusesPanel.SetActive(false);
        bonusesButtonsHorLayout.enabled = true;
    }

    private void skipAndContinue()
    {
        Off();
        GP_Analytics.Goal("skip", Globals.CurrentLevel.ToString());
        Globals.CurrentLevel++;
        if (Globals.MainPlayerData.Progress1 < Globals.CurrentLevel)
        {
            Globals.MainPlayerData.Progress1 = Globals.CurrentLevel;
            SaveLoadManager.Save();
        }
        GameManager.Instance.StartSimpleGame();
    }

    public void SetMessagingLevel(bool isON)
    {
        if (isON)
        {
            messagingPanel.SetActive(true);

            if (Globals.CurrentLevel == 0)
            {
                mainTexterText.text = Globals.lang.TutorialText;
            }
            else
            {
                mainTexterText.text = Globals.lang.LevelText + " " + Globals.CurrentLevel;
            }

            if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
            {
                mainTexterText.text = Globals.lang.CustomGameText + "!";
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
        mainTexterText.text = Globals.lang.WinText;
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
        mainTexterText.text = Globals.lang.LoseText;
        skipButtonText.text = Globals.lang.SkipText;
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

        if (Globals.CurrentLevel > 1
            && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds > Globals.REWARDED_COOLDOWN && Globals.IsPlayingSimpleGame)
        {
            skipButton.gameObject.SetActive(true);
        }
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
        bonusesButtonsHorLayout.enabled = true;
        buttonsForReward.Clear();
        mainTextRect.DOAnchorPos(Vector2.zero, 0);
        informerPanel.SetActive(false);
        bonusButton.gameObject.SetActive(false);
        bonusFVX.SetActive(false);
        scorePanel.SetActive(false);
        messagingPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        bonusesPanel.SetActive(false);
        isArrow = false;
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
        if (Globals.CurrentLevel > 0 && Globals.CurrentLevel <= 3 && isArrow)
        {
            if (arrow.activeSelf) arrow.SetActive(false);
        }

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
            float delta = 1;
            for (int i = 1; i < 12; i++)
            {
                informerImage1.sizeDelta = new Vector2(informerImage1Base.x, informerImage1Base.y * delta);
                delta -= (1f/12f);
                delta = delta > 0 ? delta : 0;
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

            case GameEventsType.random_small:
                return eventRandSmallSprite;

            case GameEventsType.random_big:
                return eventRandBigSprite;

            case GameEventsType.random_medium:
                return eventRandSmallSprite;
        }

        return null;
    }

    private IEnumerator playShake(Transform _transform)
    {
        if (isFrameShaking) yield break;
        isFrameShaking = true;
        while (true)
        {
            _transform.DOShakeScale(0.3f, 0.1f, 30).SetEase(Ease.InOutElastic);
            yield return new WaitForSeconds(1f);
            _transform.localScale = Vector3.one;
        }
    }
}
