using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static int Horizontals;
    public static int Verticals;

    public static int CurrentLevel;
    public static int Wins;

    public static string CurrentLanguage;
    public static Translation lang;



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

    public const float REWARDED_COOLDOWN = 30f;
    public const float INTERSTITIAL_COOLDOWN = 65f;

    public const float BASE_FRAME_OFFSET = 5f;
    public const float CREATE_DELETE_TIME = 0.2f;
    public const int EVENT_PACK_LIMIT = 5;
    public const int MAX_BUILDINGS = 7;
    public const int RANDOM_CHANCE = 7;
    public const float DIFFICULTY = 0.08f;
}
