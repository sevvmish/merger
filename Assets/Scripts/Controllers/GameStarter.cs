using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePush;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameStarter : MonoBehaviour
{
    public static GameStarter Instance { get; private set; }

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Ambient ambient;
    [SerializeField] private Button playButton;

    [Header("Custom Game")]
    [SerializeField] private Button customGameButton;
    [SerializeField] private GameObject rewardedIcon;
    [SerializeField] private Image customGameImage;
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Sprite activeButtonSprite;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI playButtonText;
    [SerializeField] private TextMeshProUGUI bonusButtonText;
    [SerializeField] private TextMeshProUGUI customGameButtonText;

    [Header("Advs")]
    [SerializeField] private Rewarded rewarded;
    [SerializeField] private Interstitial interstitial;

    [SerializeField] private GameObject introEnv;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform cameraTransform;


    [SerializeField] private GameObject tutorial;


    [Header("reset")]
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button resetOK;
    [SerializeField] private Button resetNO;
    [SerializeField] private TextMeshProUGUI resetText;


    private Translation lang;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        
        playButton.gameObject.SetActive(false);
        customGameButton.gameObject.SetActive(false);
        rewardedIcon.SetActive(false);
        tutorial.SetActive(false);
        resetPanel.SetActive(false);

        playButton.onClick.AddListener(() => 
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            InitSimpleGame();
        });

        resetButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            if (!resetPanel.activeSelf) resetPanel.SetActive(true);
        });

        resetOK.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            Globals.MainPlayerData.Progress1 = 0;
            SaveLoadManager.Save();
            Globals.IsInitiated = false;
            SceneManager.LoadScene("new main");
        });

        resetNO.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundsUI.click);
            if (resetPanel.activeSelf) resetPanel.SetActive(false);
        });

        customGameButton.onClick.AddListener(() =>
        {            
            if (Globals.CurrentLevel < 1)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return;
            }
                

            SoundController.Instance.PlayUISound(SoundsUI.click);

            InitCustomGame();
        });

        if (Globals.IsInitiated)
        {
            Localize();
            InitMainMenu();
        }       

    }

    
    private void startRewardedForCustomGame()
    {
        rewarded.OnRewardedEndedOK = rewardedOKForCustomGame;
        rewarded.ShowRewardedVideo();
    }

    private void rewardedOKForCustomGame()
    {
        Globals.IsCustomGameOpened = true;
        InitCustomGame();
    }

    public void StartAmbient()
    {
        ambient.SetData(AmbientType.forest);
    }

    private void Update()
    {
        if (!Globals.IsInitiated)
        {
            Globals.IsInitiated = true;

            SaveLoadManager.Load();
                        
            Globals.CurrentLanguage = GP_Language.Current().ToString();
            print("language set to: " + Globals.CurrentLanguage);

            Globals.IsMobilePlatform = GP_Device.IsMobile();
            print("platform mobile: " + Globals.IsMobilePlatform);

           

            if (GP_Platform.Type().ToString() == "YANDEX")
            {
                if (!Globals.IsMobilePlatform)
                {
                    GP_Ads.ShowSticky();                    
                }
            }
            else
            {
                print("wrong platform - " + GP_Platform.Type().ToString());
            }

            
            if (Globals.MainPlayerData.S == 1)
            {
                Globals.IsSoundOn = true;
                AudioListener.volume = 1;
            }
            else
            {
                Globals.IsSoundOn = false;
                AudioListener.volume = 0;
            }

            print("sound is: " + Globals.IsSoundOn);

            Globals.CurrentLevel = Globals.MainPlayerData.Progress1;
            

            if (Globals.TimeWhenStartedPlaying == DateTime.MinValue)
            {
                Globals.TimeWhenStartedPlaying = DateTime.Now;
                Globals.TimeWhenLastInterstitialWas = DateTime.Now;
                Globals.TimeWhenLastRewardedWas = DateTime.Now;
            }
            
            Localize();

            InitMainMenu();
        }

        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData.Progress1 = 0;
            SaveLoadManager.Save();
            Globals.IsInitiated = false;
            SceneManager.LoadScene("new main");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Globals.IsCustomGameOpened = true;
        }*/

    }

    private void InitMainMenu()
    {
        introEnv.SetActive(true);
        introEnv.transform.localScale = Vector3.zero;
        introEnv.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutSine);

        cameraTransform.DOMove(cameraPos.position, 0.3f).SetEase(Ease.OutSine);
        cameraTransform.DORotate(cameraPos.eulerAngles, 0.3f).SetEase(Ease.OutSine);

        mainMenuPanel.SetActive(true);
        playButton.gameObject.SetActive(true);
        

        if (Globals.CurrentLevel > 3)
        {            
            tutorial.SetActive(false);
        }
        else
        {
            tutorial.SetActive(true);
            tutorial.GetComponent<Tutorial>().SetTutorial();            
        }


        if (Globals.CurrentLevel >= 1)
        {
            customGameImage.sprite = activeButtonSprite;
            customGameButton.gameObject.SetActive(true);
            //customGameButton.interactable = true;
            //if (!Globals.IsCustomGameOpened) rewardedIcon.SetActive(true);
        }
        else
        {
            //customGameButton.interactable = false;
            customGameImage.sprite = inactiveButtonSprite;
            customGameButton.gameObject.SetActive(false);
        }
    }

    private void InitSimpleGame()
    {
        introEnv.SetActive(false);
        //Globals.CurrentLevel = 15;
        Globals.IsPlayingCustomGame = false;
        Globals.IsPlayingSimpleGame = true;
        mainMenuPanel.SetActive(false);

        GameManager.Instance.StartSimpleGame();
    }

    private void InitCustomGame()
    {
        introEnv.SetActive(false);
        Globals.IsPlayingCustomGame = true;
        Globals.IsPlayingSimpleGame = false;
        mainMenuPanel.SetActive(false);
        GameManager.Instance.StartCustomGame();
    }

    private void Localize()
    {
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        Globals.lang = lang;

        resetText.text = lang.ResetText;
        playButtonText.text = lang.PlayText;
        bonusButtonText.text = lang.BonusText;
        customGameButtonText.text = lang.CustomGameText;
    }
}
