using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public AssetManager GetAssets => assetManager;

    public bool IsGameStarted;
    public bool IsVisualBusy;
    public float PointerClickedCount;
    public int Score { get; private set; }
    public GameEventsType CurrentGameEventToProceed { get; private set; }
    public Action OnEventUpdated;

    private List<Frame> baseFrames;
    private HashSet<Frame> buildingToAct;
    private FrameMaker frameMaker;    
    private Dictionary<Vector2, Frame> buildingsLocations;
    private Queue<GameEventsType> gameEventsPack;

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

    public void InitTheGame()
    {
        IsGameStarted = true;

        //different inits
        OnEventUpdated = null;
        baseFrames = new List<Frame>();
        buildingToAct = new HashSet<Frame>();
        frameMaker = GetComponent<FrameMaker>();
        buildingsLocations = new Dictionary<Vector2, Frame>();
        gameEventsPack = new Queue<GameEventsType>();
        OnEventUpdated += showCurrentGameEvent;
        //===================================

        options.TurnAllOn();
        frameMaker.SetFrames(Globals.Horizontals, Globals.Verticals, baseFrames);

        for (int i = 0; i < baseFrames.Count; i++)
        {
            buildingsLocations.Add(baseFrames[i].Location, baseFrames[i]);
        }

        gameLogic.SetData(baseFrames, gameEventsPack);
        UI.SetData(gameEventsPack);
        showCurrentGameEvent();
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

        if (buildingToAct.Count > 2)
        {            
            FrameTypes newFrame = (FrameTypes)((int)lastModifiedFrame.FrameType + 1);
            foreach (var item in buildingToAct)
            {
                item.DeleteVisual(lastModifiedFrame.transform.position);
            }

            lastModifiedFrame.AddBuilding(newFrame, true);
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

    private void Update()
    {        
        if (PointerClickedCount > 0) PointerClickedCount -= Time.deltaTime;
                            
    }

    public GameEventsType getNextGameEvent()
    {
        GameEventsType result = gameEventsPack.Dequeue();
        gameLogic.UpdateState();
        return result;
    }


    public bool ReactOnFrameClick(Frame frame)
    {
        if (!frame.IsEmpty())
        {
            return false;
        }

        GameEventsType nextEvent = getNextGameEvent();
        FrameTypes isBuilding = IsEventBuildingType(nextEvent);
        if (isBuilding != FrameTypes.none)
        {
            frame.AddBuilding(isBuilding);
        }
                        
        UpdateState(frame);

        return true;
    }
   
    private void showCurrentGameEvent()
    {
        CurrentGameEventToProceed = gameEventsPack.Peek();
    }
    

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
    house_eight,
    delete_house,
    up_house
}
