using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public List<int> Progress1;
    public string L;
    public int M;
    public int S;

    public PlayerData()
    {
        Progress1 = new List<int>();
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Debug.Log("created PlayerData instance");
    }


}
