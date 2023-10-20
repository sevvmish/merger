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
        customize(Globals.CurrentLevel);
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

        if (Globals.IsPlayingSimpleGame)
        {
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
        }
        else if (Globals.IsPlayingCustomGame)
        {
            int randNumber = UnityEngine.Random.Range(0, 100);
            if (randNumber > 0 && randNumber < 4)
            {
                result = GameEventsType.random_small;
            }
            else if (randNumber >= 4 && randNumber < 8)
            {
                result = GameEventsType.random_medium;
            }
            else if (randNumber >= 8 && randNumber < 12)
            {
                result = GameEventsType.random_big;
            }
        }
        

        gameEventsArchive.Add(result);

        return result;        
    }

    public GameEventsType[] GetCurrentBonuses()
    {                        
        return bonusContainer;
    }

    private void customize(int level)
    {
        if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
        {
            level = 1000;
        }

        if (level < 5)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one };

            switch(level)
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
        else if(level < 10)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_one };
            
            if (level < 7)
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.replace_house, GameEventsType.delete_house };
            }
            else
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9,11), GameEventsType.delete_house };
            }
            
        }
        else if (level < 20)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_three };

            if (level < 15)
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.delete_house };
            }
            else
            {
                bonusContainer = new GameEventsType[] { GameEventsType.house_two, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_three, GameEventsType.delete_house };
            }
        }
        else if (level < 50)
        {
            
            
            if (level < 35)
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
        else if (level < 60)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (level < 60)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_one, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (level < 70)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house };
        }
        else if (level < 90)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four, GameEventsType.house_four };
            bonusContainer = new GameEventsType[] { GameEventsType.house_three, (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house, GameEventsType.house_five };
        }
        else if (level < 110)
        {
            levelContainer = new GameEventsType[] { GameEventsType.house_two, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_three, GameEventsType.house_four, GameEventsType.house_four, GameEventsType.house_five };
            bonusContainer = new GameEventsType[] { (GameEventsType)UnityEngine.Random.Range(3, 5), (GameEventsType)UnityEngine.Random.Range(9, 11), GameEventsType.house_four, GameEventsType.delete_house, GameEventsType.house_five };
        }
        else
        {
            print("from here");
            levelContainer = new GameEventsType[] { 
                GameEventsType.house_one, 
                GameEventsType.house_one,
                GameEventsType.house_two, 
                GameEventsType.house_two,
                GameEventsType.house_two,
                GameEventsType.house_two,
                GameEventsType.house_two,
                GameEventsType.house_two,
                GameEventsType.house_three,
                GameEventsType.house_three,
                GameEventsType.house_three,
                GameEventsType.house_three,
                GameEventsType.house_three,
                GameEventsType.house_three,
                GameEventsType.house_four,
                GameEventsType.house_four,
                GameEventsType.house_four,
                GameEventsType.house_five};

            bonusContainer = new GameEventsType[] {
                (GameEventsType)UnityEngine.Random.Range(2, 5),
                (GameEventsType)UnityEngine.Random.Range(3, 6),
                GameEventsType.delete_house, 
                GameEventsType.up_house, 
                GameEventsType.replace_house };
        }

    }

    public static int GetNeededScoreByLevel(int level)
    {
        int[] scores = new int[] {1000, 
            18,  18,  18,  22, 25, 27, 30, 30, 35, 40, //1 - 10
            45,  45,  45,  50, 60, 50, 50, 55, 60, 50, //11 - 20
            55,  60,  60,  80, 65, 65, 65, 70, 60, 60, // 21-30
            65,  65,  65,  55, 70, 60, 60, 70, 60, 60, // 31 - 40
            70,  65,  65,  65, 70, 80, 70, 70, 80, 120, // 41 - 50
            120,  120,  130,  170, 130,  140,  140,  150,  150,  220, // 51 - 60
            160,  160,  170,  170, 220,  180,  180,  190,  190,  200, // 61 - 70
            200,  210,  210,  220, 220,  230,  230,  240,  240,  240, // 71 - 80
            240,  240,  250,  250, 260,  260,  270,  270,  280,  280, // 81 - 90
            280,  280,  280,  280, 290,  290,  290,  300,  300,  350, // 91 - 100
            310,  310,  320,  320, 330,  330,  340,  340,  350,  350, // 101 - 110
            360,  370,  380,  390, 400,  410,  420,  430,  440,  450 // 111 - 120
        };

        if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame) return int.MaxValue;
        if (level > 120) return 500 + Globals.Wins * 10;

        int result = scores[level];

        if (Globals.CurrentLevel > 10)
        {
            result = (int)(result * (1f + Globals.Wins * Globals.DIFFICULTY));
        }

        return result;
    }

    public static int GetNeededBonusByLevel(int level)
    {
        int[] scores = new int[] { 500, 
            6,  6,   6,  10, 12, 12, 12, 12, 12, 15, //1-10
            20,  20,  20, 22, 25, 22, 22, 22, 25, 22, //11-20
            22,  22,  22, 25, 25, 25, 25, 25, 25, 25, //21-30
            22,  22,  22, 25, 25, 25, 25, 25, 25, 25, //31-40
            25,  25,  25, 25, 25, 25, 25, 25, 25, 30, //41-50
            30,  30,  30, 30, 30, 30, 30, 30, 30, 30, //51-60
            30,  30,  30, 40, 40, 40, 40, 40, 40, 40, //61-70
            50,  50,  50, 50, 50, 50, 50, 50, 50, 50, //71-80
            50,  50,  50, 50, 50, 50, 50, 50, 50, 50, //81-90
            50,  50,  50, 50, 50, 50, 50, 50, 50, 50, //91-100
            60,  60,  60, 60, 60, 60, 60, 60, 60, 60 //101-110
        };

        if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame) return 60;

        if (level > 105) return 60;

        return scores[level];
    }

}
