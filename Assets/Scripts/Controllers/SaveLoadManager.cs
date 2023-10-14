using UnityEngine;
using System.Linq;
using GamePush;

public class SaveLoadManager
{
    
    private const string ID = "Player29";

    public static void Save()
    {        
        Globals.MainPlayerData.L = Globals.CurrentLanguage;
        Globals.MainPlayerData.M = Globals.IsMobilePlatform ? 1 : 0;
        Globals.MainPlayerData.S = Globals.IsSoundOn ? 1 : 0;

        string data = JsonUtility.ToJson(Globals.MainPlayerData);
        //Debug.Log("saved: " + data);
        PlayerPrefs.SetString(ID, data);

        //YandexGame.savesData.PlayerMainData1 = data;

        try
        {
            GP_Player.Set("save", data);
            GP_Player.Sync();
            //YandexGame.SaveProgress();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            Debug.LogError("error saving data, defaults loaded");            
        }        
    }


    public static void Load()
    {
        string fromSave = "";
        //YandexGame.LoadProgress();

        try
        {
            if (GP_Player.Has("save"))
            {
                Debug.Log("yes? have key");
                fromSave = GP_Player.GetString("save");

                if (string.IsNullOrEmpty(fromSave))
                {
                    Globals.MainPlayerData = new PlayerData();
                }
                else
                {
                    Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
                }

                
                Debug.Log("result - "+fromSave);
            }
            else
            {
                fromSave = PlayerPrefs.GetString(ID);

                if (string.IsNullOrEmpty(fromSave))
                {
                    Globals.MainPlayerData = new PlayerData();
                }
                else
                {
                    Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
                }
                
            }
            
            
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            Debug.LogError("error loading data, defaults loaded");
            Globals.MainPlayerData = new PlayerData();
        }
            
          /*  
        if (!string.IsNullOrEmpty(fromSave))
        {
            

            Debug.Log("loaded: " + fromSave);
            try
            {
                Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
            }
            catch (System.Exception)
            {
                Globals.MainPlayerData = new PlayerData();
            }
                        
        }
        else
        {
            
            fromSave = PlayerPrefs.GetString(ID);
                        
            if (string.IsNullOrEmpty(fromSave))
            {
                Globals.MainPlayerData = new PlayerData();
            }
            else
            {
                Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
            }
        }       */
    }

}
