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

        for(int i=0; i<1000;i++)
        {
            result = levelContainer[UnityEngine.Random.Range(0, levelContainer.Length)];

            if (gameEventsArchive.Count > 3 && result == gameEventsArchive[gameEventsArchive.Count - 1]
                && result == gameEventsArchive[gameEventsArchive.Count - 2]
                && result == gameEventsArchive[gameEventsArchive.Count - 3])
            {
                //
            }
            else
            {
                break;
            }
        }

        if (gameEventsArchive.Count > 3 && gameEventsArchive[gameEventsArchive.Count - 1] != GameEventsType.random_small
                && gameEventsArchive[gameEventsArchive.Count - 2] != GameEventsType.random_small
                && gameEventsArchive[gameEventsArchive.Count - 3] != GameEventsType.random_small)
        {
            int randNumber = UnityEngine.Random.Range(0, 100);
            if (Globals.CurrentLevel > 20 && Globals.CurrentLevel <= 50)
            {
                if (randNumber < Globals.RANDOM_CHANCE)
                {
                    result = GameEventsType.random_small;
                }
            }            
        }

        if (gameEventsArchive.Count > 3 && gameEventsArchive[gameEventsArchive.Count - 1] != GameEventsType.random_medium
                && gameEventsArchive[gameEventsArchive.Count - 2] != GameEventsType.random_medium
                && gameEventsArchive[gameEventsArchive.Count - 3] != GameEventsType.random_medium)
        {
            int randNumber = UnityEngine.Random.Range(0, 100);
            if (Globals.CurrentLevel > 50 && Globals.CurrentLevel <= 100)
            {
                if (randNumber < Globals.RANDOM_CHANCE)
                {
                    result = GameEventsType.random_medium;
                }
            }
        }

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
        else if (Globals.CurrentLevel < 20)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_three };

            if (Globals.CurrentLevel < 15)
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.delete_house };
            }
            else
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_three, GameEventsType.delete_house };
            }
        }
        else if (Globals.CurrentLevel < 50)
        {
            
            
            if (Globals.CurrentLevel < 35)
            {
                levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_one, GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three };
                bonusContainer = new GameEventsType[] { GameEventsType.house_two,  (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_three, GameEventsType.delete_house };
                
            }
            else
            {
                levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three };
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.up_house, GameEventsType.delete_house, GameEventsType.house_three,   GameEventsType.replace_house };
            }
        }
        else if (Globals.CurrentLevel < 60)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (Globals.CurrentLevel < 60)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (Globals.CurrentLevel < 70)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (Globals.CurrentLevel < 80)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house, GameEventsType.house_five };
        }

    }

    public static int GetNeededScoreByLevel(int level)
    {
        int[] scores = new int[] {1000, 
            18,  18,  18,  30, 30, 35, 30, 30, 35, 40, //1 - 10
            45,  45,  45,  50, 60, 50, 50, 55, 60, 50, //11 - 20
            55,  60,  60,  80, 65, 65, 65, 70, 60, 60, // 21-30
            65,  65,  65,  65, 80, 70, 70, 80, 70, 70, // 31 - 40
            80,  75,  75,  75, 80, 90, 80, 80, 90, 120, // 41 - 50
            120,  120,  120,  130, 150,  130,  140,  140,  140,  150, // 51 - 60
            150,  150,  170,  160, 160,  160,  160,  180,  160,  160, // 61 - 70
            160,  170,  170,  170, 170,  170,  170,  180,  180,  180 // 71 - 80

        };

        return scores[level];
    }

    public static int GetNeededBonusByLevel(int level)
    {
        int[] scores = new int[] { 500, 
            10,  10,  10, 12, 12, 12, 12, 12, 12, 15, //1-10
            20,  20,  20, 22, 25, 22, 22, 22, 25, 22, //11-20
            22,  22,  22, 25, 25, 25, 25, 25, 25, 25, //21-30
            22,  22,  22, 25, 25, 25, 25, 25, 25, 25, //31-40
            25,  25,  25, 25, 25, 25, 25, 25, 25, 30, //41-50
            30,  30,  30, 30, 30, 30, 30, 30, 30, 30, //51-60
            30,  30,  30, 40, 40, 40, 40, 40, 40, 40, //61-70
            40,  40,  40, 40, 40, 40, 40, 40, 40, 40, //71-80
        };

        return scores[level];
    }

}
