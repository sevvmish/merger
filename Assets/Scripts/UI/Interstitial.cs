using System;
using UnityEngine;
using GamePush;

public class Interstitial : MonoBehaviour
{
    public Action OnEnded;

    private void Start()
    {        
        GP_Ads.OnFullscreenStart += advStarted;
        GP_Ads.OnFullscreenClose += advClosed;
    }

    

    public void ShowInterstitialVideo()
    {
        GP_Ads.ShowFullscreen(advStarted, advClosed);        
    }

    private void advStarted()
    {
        print("interstitial staarted OK");
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }
    }


    private void advClosed(bool isOK)
    {
        print("interstitial was closed");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        Globals.TimeWhenLastInterstitialWas = DateTime.Now;

        OnEnded?.Invoke();
    }
}
