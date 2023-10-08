using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarded : MonoBehaviour
{
    public Action OnRewardedEndedOK;
    public Action OnError;
        
    private bool isRewardedOK;
    
    public void ShowRewardedVideo()
    {
        isRewardedOK = false;

        //YandexGame.OpenVideoEvent = rewardStarted;
        //YandexGame.RewardVideoEvent = rewardedClosedOK;
        //YandexGame.CloseVideoEvent = advRewardedClosed;
        //YandexGame.ErrorVideoEvent = advRewardedError;
        //YandexGame.RewVideoShow(155);
        
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

    private void rewardedClosedOK(int value)
    {
        //155
        if (value == 155)
        {
            isRewardedOK = true;
        }

        Globals.TimeWhenLastRewardedWas = DateTime.Now;
    }

    private void advRewardedClosed()
    {
        //print("rewarded was closed ok");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        if (isRewardedOK)
        {
            OnRewardedEndedOK?.Invoke();
        }
        else
        {
            OnError?.Invoke();
        }           
        
    }

    private void advRewardedError()
    {
        //print("rewarded was ERROR!");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        OnError?.Invoke();
        
    }
}
