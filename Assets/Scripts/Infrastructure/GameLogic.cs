using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private List<Frame> baseFrames = new List<Frame>();
    private Queue<GameEventsType> gameEventsPack = new Queue<GameEventsType>();
    private List<GameEventsType> gameEventsArchive = new List<GameEventsType>();

    private int stateLimitForLevel;
    private GameManager gm;

    private GameEventsType[] levelContainer;
    private GameEventsType[] bonusContainer;

    public void SetData(List<Frame> baseFrames, Queue<GameEventsType> gameEventsPack)
    {
        gm = GameManager.Instance;
        customize();
        this.baseFrames = baseFrames;
        this.gameEventsPack = gameEventsPack;
        gameEventsArchive.Clear();
        this.gameEventsPack.Clear();

        for (int i = 0; i < Globals.EVENT_PACK_LIMIT; i++)
        {
            this.gameEventsPack.Enqueue(GetCurrentEvent());
        }
    }

    public void UpdateState()
    {
        if (gameEventsPack.Count < Globals.EVENT_PACK_LIMIT)
        {
            for (int i = 0; i < (Globals.EVENT_PACK_LIMIT - gameEventsPack.Count); i++)
            {
                gameEventsPack.Enqueue(GetCurrentEvent());
                gm.OnEventUpdated.Invoke();
            }
        }
    }

    private GameEventsType GetCurrentEvent()
    {
        GameEventsType result = GameEventsType.house_one;

        result = levelContainer[UnityEngine.Random.Range(0, levelContainer.Length)];

        gameEventsArchive.Add(result);

        return result;        
    }

    private GameEventsType GetCurrentBonus()
    {
        GameEventsType result = GameEventsType.house_one;

        result = (GameEventsType)UnityEngine.Random.Range(4, 11);

        gameEventsArchive.Add(result);

        return result;
    }

    private void customize()
    {
        if (Globals.CurrentLevel < 5)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one };
            bonusContainer = new GameEventsType[] { GameEventsType.house_one };
        }
        else if(Globals.CurrentLevel < 10)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two };
            bonusContainer = new GameEventsType[] { GameEventsType.house_one };
        }
        else if (Globals.CurrentLevel < 15)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two , GameEventsType.house_three};
            bonusContainer = new GameEventsType[] { GameEventsType.house_one };
        }
        else if (Globals.CurrentLevel < 20)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one };
            bonusContainer = new GameEventsType[] { GameEventsType.house_one };
        }
    }

    public static int GetNeededScoreByLevel(int level)
    {
        int[] scores = new int[] {20, 50, 100, 150, 200, 250, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300 };

        return scores[level];
    }

    public static int GetNeededBonusByLevel(int level)
    {
        int[] scores = new int[] { 20, 30, 40, 50, 60, 70, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };

        return scores[level];
    }

}
