using DG.Tweening;
using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AssetManager assetManager;
    [SerializeField] private OptionsMenu options;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private UIManager UI;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private InputController inputController;

    public AssetManager GetAssets => assetManager;    
    public bool IsGameStarted = false;
    public bool IsVisualBusy;
    public bool IsInputOn = true;
    public float PointerClickedCount;
    public List<Frame> GetBaseFrames => baseFrames;
    public GameEventsType[] GetCurrentBonuses => gameLogic.GetCurrentBonuses();

    public int Score { get; private set; }
    public float ScoreProgress => (float)Score / ((float)neededScore + 0.001f);
    public float BonusProgress => (float)CurrentBonus / ((float)bonusNeeded + 0.001f);
    public int CurrentBonus {
        get { return bonus; }
        set { 
            if (value < bonusNeeded)
            {
                bonus = value;
            }
            else
            {
                bonus = bonusNeeded;
            }
        }
    }
    private int bonus;
    private int neededScore;
    private int bonusNeeded;

    public Queue<GameEventsType> GetCurrentEventPack => gameEventsPack;

    public GameEventsType CurrentGameEventToProceed => gameEventsPack.Peek();
    public Action OnEventUpdated;
    public Action OnGameEnded;

    private List<Frame> baseFrames;
    private HashSet<Frame> buildingToAct;
    private FrameMaker frameMaker;    
    private Dictionary<Vector2, Frame> buildingsLocations;
    private Queue<GameEventsType> gameEventsPack;
    private Frame frameToReplaceForReplacement;

    private float _timer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartCustomGame()
    {
        StartCoroutine(playLevelIntro());
    }

    public void StartSimpleGame()
    {
        StartCoroutine(playLevelIntro());
    }
    private IEnumerator playLevelIntro()
    {
        UI.SetMessagingLevel(true);
        yield return new WaitForSeconds(2);
        UI.SetMessagingLevel(false);
        initTheGame();
    }

    public void CheckFramesForError()
    {
        if (Globals.CurrentLevel == 0) return;

        for (int i = 0; i < baseFrames.Count; i++)
        {
            if (!baseFrames[i].IsEmpty())
            {
                baseFrames[i].CheckBuilding();
            }
        }
    }

    public void Restart()
    {
        stopTheGame();

        if (Globals.IsPlayingSimpleGame && !Globals.IsPlayingCustomGame)
        {
            StartSimpleGame();
        }
        else if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
        {
            StartCustomGame();
        }
    }

    public void StopForTutorial()
    {
        stopTheGame();
    }

    private void stopTheGame()
    {
        GP_Game.GameplayStop();
        IsGameStarted = false;
        OnEventUpdated = null;
        options.TurnAllOff();
        frameMaker.Off();
        UI.Off();
    }

    private void initTheGame()
    {
        GP_Game.GameplayStart();
        if (Globals.IsMobilePlatform && Globals.CurrentLevel > 9 && !GP_Ads.IsStickyPlaying())
        {
            GP_Ads.ShowSticky();
        }
        GameStarter.Instance.StartAmbient();
        if (Globals.IsSoundOn) StartCoroutine(muteLittle());

        IsGameStarted = true;
        _timer = 0;
        Score = 0;
        bonus = 0;
        IsVisualBusy = false;
        PointerClickedCount = 0;

        //different inits
        frameToReplaceForReplacement = null;
        inputController.SetData(cameraMain);
        OnEventUpdated = null;
        baseFrames = new List<Frame>();
        buildingToAct = new HashSet<Frame>();
        frameMaker = GetComponent<FrameMaker>();
        buildingsLocations = new Dictionary<Vector2, Frame>();
        gameEventsPack = new Queue<GameEventsType>();
        //OnEventUpdated += showCurrentGameEvent;
        neededScore = GameLogic.GetNeededScoreByLevel(Globals.CurrentLevel);
        //print(neededScore + " !!!!!!!!!!!!!!!!!!!!!!!!!");
        bonusNeeded = GameLogic.GetNeededBonusByLevel(Globals.CurrentLevel);
        //===================================

        options.TurnAllOn();
        frameMaker.SetFrames(baseFrames, cameraMain);

        for (int i = 0; i < baseFrames.Count; i++)
        {
            buildingsLocations.Add(baseFrames[i].Location, baseFrames[i]);
        }

        gameLogic.SetData(baseFrames, gameEventsPack);
        UI.SetData(gameEventsPack);
        //showCurrentGameEvent();
    }
    private IEnumerator muteLittle()
    {
        AudioListener.volume = 0;
        yield return new WaitForSeconds(0.2f);
        AudioListener.volume = 1;
    }
    
    
    public void AddBonus(GameEventsType bonus)
    {
        List<GameEventsType> currentBonuses = new List<GameEventsType>(gameEventsPack.ToArray());
        currentBonuses.Insert(0, bonus);
        gameEventsPack.Clear();

        for (int i = 0; i < currentBonuses.Count-1; i++)
        {
            gameEventsPack.Enqueue(currentBonuses[i]);            
        }

        
        /*
        if (bonus == GameEventsType.delete_house || bonus == GameEventsType.up_house || bonus == GameEventsType.replace_house)
        {
            //CurrentGameEventToProceed = gameEventsPack.Peek();
            getNextGameEvent();
        }        
        else
        {
            //CurrentGameEventToProceed = gameEventsPack.Peek();
            getNextGameEvent();
        }*/
    }
    

    public void UpdateState(Frame lastModifiedFrame)
    {
        if (lastModifiedFrame.IsEmpty()) return;

        buildingToAct.Clear();

        Frame frame = lastModifiedFrame;
        FrameTypes _type = frame.FrameType;
        buildingToAct.Add(frame);

        searchAllConnections(frame, buildingToAct);

        //print(buildingToAct.Count);
        int scoreCount = 0;

        if (buildingToAct.Count > 2)
        {            
            FrameTypes newFrame = (FrameTypes)((int)lastModifiedFrame.FrameType + 1);
            foreach (var item in buildingToAct)
            {
                scoreCount += (int)item.FrameType;
                item.DeleteVisual(lastModifiedFrame.transform.position);
            }

            //print("score: " + scoreCount);
            Score += scoreCount;
            CurrentBonus += scoreCount;
            //print("SCORE: " + Score);
            UI.ShowBonusAddedText(scoreCount);
            if (newFrame != (FrameTypes)8) lastModifiedFrame.AddBuilding(newFrame, true, buildingToAct.Count);
            UpdateState(lastModifiedFrame);
        }
    }

    private void searchAllConnections(Frame frame, HashSet<Frame> allSet)
    {
        //check up vertical
        for (int j = 1; j < Globals.Horizontals; j++)
        {
            Vector2 vec = frame.Location + Vector2.up * j * Globals.BASE_FRAME_OFFSET;

            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Horizontals; j++)
        {
            Vector2 vec = frame.Location + Vector2.down * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Verticals; j++)
        {
            Vector2 vec = frame.Location + Vector2.left * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Verticals; j++)
        {
            Vector2 vec = frame.Location + Vector2.right * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }
    }

    public void SpendBonusActivated()
    {
        if (CurrentBonus < bonusNeeded) return;

        CurrentBonus = 0;
    }

    private void Update()
    {        
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            Score += 1000000;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < baseFrames.Count; i++)
            {
                print(i + ": " + baseFrames[i].IsEmpty());
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            print("curr: " + CurrentGameEventToProceed);

            int index = 0;
            foreach (var item in gameEventsPack)
            {
                print(index + ": " + item);
                index++;
            }
        }*/


        if (PointerClickedCount > 0) PointerClickedCount -= Time.deltaTime;

        if (_timer > 0.3f && IsGameStarted && !IsVisualBusy && PointerClickedCount <= 0 && Globals.CurrentLevel > 0)
        {
            _timer = 0;

            if (Score >= neededScore)
            {
                Globals.CurrentLevel++;
                Globals.Wins++;

                if (Globals.Wins > 9)
                {
                    int chance = UnityEngine.Random.Range(0, 100);

                    if (chance < 15)
                    {
                        Globals.Wins = 5;
                    }
                    else if (chance < 30)
                    {
                        Globals.Wins = 3;
                    }
                }

                if (Globals.MainPlayerData.Progress1 < Globals.CurrentLevel && !Globals.IsPlayingCustomGame) 
                {
                    Globals.MainPlayerData.Progress1 = Globals.CurrentLevel;
                    
                    if (Globals.CurrentLevel % 5 == 0)
                    {
                        GP_Analytics.Goal("level" + Globals.CurrentLevel, "");
                    }
                    
                    
                    SaveLoadManager.Save();
                }
                
                stopTheGame();
                UI.SetMessagingWin();
                return;
            }


            bool isOK = true;
            for (int i = 0; i < baseFrames.Count; i++)
            {
                if (baseFrames[i].IsEmpty())
                {
                    isOK = false;
                    break;
                }
            }

            if (isOK && Score < neededScore)
            {
                //GP_Analytics.Goal("lost", Globals.CurrentLevel.ToString());
                stopTheGame();
                UI.SetMessagingLose();
                
                if (Globals.Wins > 1)
                {
                    Globals.Wins /= 2;
                }
                else
                {
                    Globals.Wins = 0;
                }
                
                return;
            }
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public GameEventsType getNextGameEvent()
    {
        GameEventsType result = gameEventsPack.Dequeue();
        gameLogic.UpdateState();
        return result;
    }


    public bool ReactOnFrameClick(Frame frame)
    {
        if ((int)CurrentGameEventToProceed>0 && (int)CurrentGameEventToProceed <= 7)
        {
            if (!frame.IsEmpty())
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return false;
            }

            GameEventsType nextEvent = getNextGameEvent();
            FrameTypes isBuilding = IsEventBuildingType(nextEvent);
            if (isBuilding != FrameTypes.none)
            {
                frame.AddBuilding(isBuilding);
            }

            //UpdateState(frame);
            StartCoroutine(playUpdateStateAfterSec(Globals.CREATE_DELETE_TIME, frame));

            return true;
        }

        if (CurrentGameEventToProceed == GameEventsType.delete_house)
        {
            bool isOK = false;
            for (int i = 0; i < baseFrames.Count; i++)
            {
                if (!baseFrames[i].IsEmpty())
                {
                    isOK = true;
                    break;
                }
            }
            if (!isOK)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                getNextGameEvent();
                return true;
            }

            //========================

            if (frame.IsEmpty())
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return false;
            }

            

            GameEventsType nextEvent = getNextGameEvent();
            frame.DeleteBuilding();
            UpdateState(frame);
            return true;
        }

        if (CurrentGameEventToProceed == GameEventsType.up_house)
        {
            bool isOK = false;
            for (int i = 0; i < baseFrames.Count; i++)
            {
                if (!baseFrames[i].IsEmpty() && (int)baseFrames[i].FrameType < (int)FrameTypes.seven)
                {
                    isOK = true;
                    break;
                }
            }
            if (!isOK)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                getNextGameEvent();
                return true;
            }

            //===========================


            if (frame.IsEmpty() || frame.FrameType == FrameTypes.seven)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return false;
            }

            

            GameEventsType nextEvent = getNextGameEvent();
            frame.UpdateBuilding();
            StartCoroutine(playUpdateStateAfterSec(Globals.CREATE_DELETE_TIME, frame));
            return true;
        }

        if (CurrentGameEventToProceed == GameEventsType.replace_house)
        {
            bool isOK = false;
            for (int i = 0; i < baseFrames.Count; i++)
            {
                if (!baseFrames[i].IsEmpty())
                {
                    isOK = true;
                    break;
                }
            }
            if (!isOK)
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                getNextGameEvent();
                return true;
            }

            //=============================

            
            if (frameToReplaceForReplacement == null)
            {
                if (frame.IsEmpty())
                {
                    SoundController.Instance.PlayUISound(SoundsUI.error);
                    return false;
                }

                frameToReplaceForReplacement = frame;
                frame.SetShake(true);
            }
            else
            {
                if (frame.IsEmpty())
                {
                    FrameTypes newFrame = frameToReplaceForReplacement.FrameType;
                    frameToReplaceForReplacement.DeleteBuilding();
                    frame.AddBuilding(newFrame);
                }
                else if (!frame.IsEmpty() && frame.FrameType == frameToReplaceForReplacement.FrameType)
                {
                    SoundController.Instance.PlayUISound(SoundsUI.error);
                    return false;
                }
                else
                {
                    FrameTypes newFrame = frameToReplaceForReplacement.FrameType;
                    frameToReplaceForReplacement.ChangeBuilding(frame.FrameType);
                    frame.ChangeBuilding(newFrame);
                }


                StartCoroutine(playUpdateStateAfterSec(Globals.CREATE_DELETE_TIME * 2f,frame));
                getNextGameEvent();
                frameToReplaceForReplacement = null;
                return true;
            }
        }

        if (CurrentGameEventToProceed == GameEventsType.random_small || CurrentGameEventToProceed == GameEventsType.random_big || CurrentGameEventToProceed == GameEventsType.random_medium)
        {
            if (!frame.IsEmpty())
            {
                SoundController.Instance.PlayUISound(SoundsUI.error);
                return false;
            }

            frame.AddRandom(CurrentGameEventToProceed);
            getNextGameEvent();
            StartCoroutine(playUpdateStateAfterSec(Globals.CREATE_DELETE_TIME * 2.2f, frame));
            return true;
        }

        return true;
    }

    private IEnumerator playUpdateStateAfterSec(float secs, Frame frame)
    {
        //IsVisualBusy = true;
        yield return new WaitForSeconds(secs);
        UpdateState(frame);
        //IsVisualBusy = false;
    }


    //private void showCurrentGameEvent()
    //{
     //   CurrentGameEventToProceed = gameEventsPack.Peek();
    //}
    

    private IEnumerator playShake(Transform _transform)
    {
        while (true)
        {
            _transform.DOShakeScale(0.5f, 0.3f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(1.6f);

            //transform.DOPunchScale(Vector3.one*0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);

            //transform.DOPunchPosition(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);
        }
    }

    public static FrameTypes IsEventBuildingType(GameEventsType _type)
    {
        switch(_type)
        {
            case GameEventsType.house_one:
                return FrameTypes.one;

            case GameEventsType.house_two:
                return FrameTypes.two;

            case GameEventsType.house_three:
                return FrameTypes.three;

            case GameEventsType.house_four:
                return FrameTypes.four;

            case GameEventsType.house_five:
                return FrameTypes.five;

            case GameEventsType.house_six:
                return FrameTypes.six;

            case GameEventsType.house_seven:
                return FrameTypes.seven;
        }

        return FrameTypes.none;
    }
}

public enum GameEventsType
{
    none,
    house_one,
    house_two,
    house_three,
    house_four,
    house_five,
    house_six,
    house_seven,
    delete_house,
    up_house,
    replace_house,
    random_small,
    random_big,
    random_medium
}
