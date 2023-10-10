using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public static GameStarter Instance { get; private set; }

    [SerializeField] private Ambient ambient;

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

        SaveLoadManager.Load();

        Globals.IsSoundOn = Globals.MainPlayerData.S == 1 ? true : false;

        for (int i = 0; i < Globals.MainPlayerData.Progress1.Count; i++)
        {
            if (Globals.MainPlayerData.Progress1[i] == 0)
            {
                Globals.CurrentLevel = i;
                break;
            }
        }

        SaveLoadManager.Save();

        Globals.IsInitiated = true;

        if (Globals.IsSoundOn)
        {
            StartAmbient();
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

        GameManager.Instance.InitTheGame();
        
    }

    public void StartAmbient()
    {
        ambient.SetData(AmbientType.forest);
    }

    
}
