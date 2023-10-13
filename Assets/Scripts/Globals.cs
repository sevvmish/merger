using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static int Horizontals = 5;
    public static int Verticals = 5;

    public static int CurrentLevel = 0;

    public static string CurrentLanguage = "ru";
    public static bool IsMobilePlatform = false;
    public static bool IsSoundOn;
    public static bool IsInitiated = false;
    public static bool IsInitiatedPlayer = false;
    public static bool IsCustomGameOpened = false;
    public static bool IsPlayingSimpleGame = false;
    public static bool IsPlayingCustomGame = false;
    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public static DateTime TimeWhenLastRewardedInMainMenuWas;

    public const float REWARDED_COOLDOWN = 65f;
    public const float INTERSTITIAL_COOLDOWN = 65f;

    public const float BASE_FRAME_OFFSET = 5f;
    public const float CREATE_DELETE_TIME = 0.25f;
    public const int EVENT_PACK_LIMIT = 5;
    public const int MAX_BUILDINGS = 7;
}
