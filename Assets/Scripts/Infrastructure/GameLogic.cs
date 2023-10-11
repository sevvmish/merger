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

    public void SetData(List<Frame> baseFrames, Queue<GameEventsType> gameEventsPack)
    {
        gm = GameManager.Instance;

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
        GameEventsType result = (GameEventsType)UnityEngine.Random.Range(1, 5);
        
        return result;
        /*
        switch(Globals.CurrentLevel)
        {
            case 0:
                return GameEventsType.house_one;
                break;
        }*/

        return GameEventsType.none;
    }

    public static int GetNeededScoreByLevel(int level)
    {
        int[] scores = new int[] {1200, 50, 100, 150, 200 };

        return scores[level];
    }

    public static int GetNeededBonusByLevel(int level)
    {
        int[] scores = new int[] { 20, 30, 40, 50, 60 };

        return scores[level];
    }

}
