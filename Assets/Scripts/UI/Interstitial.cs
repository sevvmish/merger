using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial : MonoBehaviour
{
    public Action OnEnded;

    public void ShowInterstitialVideo()
    {
        //YandexGame.OpenFullAdEvent = advStarted;
        //YandexGame.CloseFullAdEvent = advClosed;//nextLevelAction;
        //YandexGame.ErrorFullAdEvent = advError;//nextLevelAction;
        //YandexGame.FullscreenShow();
        //print("starting to show Interstitial");
        
    }

    private void advStarted()
    {
        //print("interstitial staarted OK");
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }
    }

    private void advError()
    {
        //print("interstitial was ERROR");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }
        OnEnded?.Invoke();
    }

    private void advClosed()
    {
        //print("interstitial was closed");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        Globals.TimeWhenLastInterstitialWas = DateTime.Now;

        OnEnded?.Invoke();
    }
}
