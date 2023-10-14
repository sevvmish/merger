using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePush;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        playButton.onClick.AddListener(() => 
        {
            InitSimpleGame();
        });

        customGameButton.onClick.AddListener(() =>
        {            
            if (Globals.CurrentLevel < 1) return;

            if (Globals.IsCustomGameOpened)
            {
                InitCustomGame();
            }
            else
            {
                startRewardedForCustomGame();
            }            
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
                //if (!Globals.IsMobilePlatform)
                //{
                    GP_Ads.ShowSticky();
                //}
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
                Globals.TimeWhenLastRewardedInMainMenuWas = DateTime.Now;
            }

            Localize();

            InitMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData.Progress1 = 0;
            SaveLoadManager.Save();
            Globals.IsInitiated = false;
            SceneManager.LoadScene("new main");
        }
        
    }

    private void InitMainMenu()
    {
        mainMenuPanel.SetActive(true);
        playButton.gameObject.SetActive(true);
        customGameButton.gameObject.SetActive(true);

        if (Globals.CurrentLevel > 0)
        {
            customGameImage.sprite = activeButtonSprite;
            rewardedIcon.SetActive(true);
        }
        else
        {
            customGameImage.sprite = inactiveButtonSprite;
        }
    }

    private void InitSimpleGame()
    {
        Globals.IsPlayingCustomGame = false;
        Globals.IsPlayingSimpleGame = true;
        mainMenuPanel.SetActive(false);        
        GameManager.Instance.StartSimpleGame();
    }

    private void InitCustomGame()
    {
        Globals.IsPlayingCustomGame = true;
        Globals.IsPlayingSimpleGame = false;
        mainMenuPanel.SetActive(false);
        GameManager.Instance.StartCustomGame();
    }

    private void Localize()
    {
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        playButtonText.text = lang.PlayText;
        bonusButtonText.text = lang.BonusText;
        customGameButtonText.text = lang.CustomGameText;
    }
}
