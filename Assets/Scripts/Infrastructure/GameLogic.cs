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

    public GameEventsType[] GetCurrentBonuses()
    {                        
        return bonusContainer;
    }

    private void customize()
    {
        if (Globals.CurrentLevel < 5)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one };

            switch(Globals.CurrentLevel)
            {
                case 1:
                    bonusContainer = new GameEventsType[] { GameEventsType.up_house};
                    break;
                case 2:
                    bonusContainer = new GameEventsType[] { GameEventsType.delete_house};
                    break;
                case 3:
                    bonusContainer = new GameEventsType[] { GameEventsType.replace_house};
                    break;
                case 4:
                    bonusContainer = new GameEventsType[] { GameEventsType.replace_house, GameEventsType.up_house };
                    break;
                
            }
        }
        else if(Globals.CurrentLevel < 10)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_one };
            
            if (Globals.CurrentLevel < 7)
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.replace_house, GameEventsType.delete_house };
            }
            else
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9,11), GameEventsType.delete_house };
            }
            
        }
        else if (Globals.CurrentLevel < 60)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two , GameEventsType.house_three, GameEventsType.house_one, GameEventsType.house_two };
            
            if (Globals.CurrentLevel < 35)
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.delete_house };
            }
            else
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, GameEventsType.up_house, GameEventsType.replace_house, GameEventsType.delete_house };
            }
        }
       
    }

    public static int GetNeededScoreByLevel(int level)
    {
        int[] scores = new int[] {1000, 
            18,  18,  18,  30, 30, 35, 30, 30, 30, 40, //1 - 10
            40,  40,  50,  40, 45, 45, 45, 45, 45, 50, //11 - 20
            300, 300, 300, 300, 300, 300, 300 };

        return scores[level];
    }

    public static int GetNeededBonusByLevel(int level)
    {
        int[] scores = new int[] { 500, 
            10,  10,  10, 12, 12, 12, 12, 12, 12, 15, //1-10
            15,  15,  15, 18, 20, 20, 20, 20, 20, 20, //11-20
            80, 80, 80, 80, 80, 80, 80, 80, 80, 80, //21-30
            80, 80, 80 };

        return scores[level];
    }

}
