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
        GameEventsType result = (GameEventsType)UnityEngine.Random.Range(1, Globals.MAX_BUILDINGS+1);
        
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

}
