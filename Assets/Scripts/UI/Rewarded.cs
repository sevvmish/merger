using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePush;

public class Rewarded : MonoBehaviour
{
    public Action OnRewardedEndedOK;
    public Action OnError;
 
    private void Start()
    {
        GP_Ads.OnRewardedStart += rewardStarted;
        GP_Ads.OnRewardedReward += rewardedClosedOK;
        GP_Ads.OnRewardedClose += advRewardedClosed;
    }

    public void ShowRewardedVideo()
    {        
        GP_Ads.ShowRewarded("rew");        
    }

    private void rewardStarted()
    {        
        //print("reward started OK");
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }
    }

    private void rewardedClosedOK(string value)
    {
        //155
        if (value == "rew")
        {
            
        }

        Globals.TimeWhenLastRewardedWas = DateTime.Now;
    }

    private void advRewardedClosed(bool isOK)
    {
        //print("rewarded was closed ok");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        if (isOK)
        {
            OnRewardedEndedOK?.Invoke();
        }
        else
        {
            OnError?.Invoke();
        }        
    }

}
