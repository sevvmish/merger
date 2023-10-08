using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssetManager : MonoBehaviour
{
    [Header("Vehicles")]
    public GameObject Taxi;
    public GameObject Van;
    public GameObject Ambulance;

    [Header("Vehicles signes")]
    public GameObject TaxiSign;
    public GameObject VanSign;
    public GameObject AmbulanceSign;
    public ObjectPool TaxiSignPool;
    public ObjectPool VanSignPool;
    public ObjectPool AmbulanceSignPool;

    [Header("MISC")]
    public GameObject CarDestroEffect;
    public ObjectPool CarDestroEffectPool;

    [Header("Vehicles pools")]
    public ObjectPool TaxiPool;
    public ObjectPool VanPool;
    public ObjectPool AmbulancePool;

    [Header("Sounds")]
    public AudioClip ErrorClip;
    public AudioClip SmallTownAmbient;
    public AudioClip ForestAmbient;
    public AudioClip DesertAmbient;
    public AudioClip hornClip;
    public AudioClip hornDoubleClip;
    public AudioClip ambuSirenClip;
    public AudioClip sirenClip;
    public AudioClip specSignalClip;
    public AudioClip motorSoundLightClip;
    public AudioClip motorSoundHeavyClip;
    public AudioClip positiveSoundClip;
    public AudioClip ErrorBiggerClip;
    public AudioClip Swallow;
    public AudioClip Tick;
    public AudioClip Pop;
    public AudioClip Click;
    public AudioClip Win;
    public AudioClip Lose;


    public GameObject GetVehicle(Vehicles vehicle)
    {
        switch(vehicle)
        {
            case Vehicles.taxi:
                return Taxi;

            case Vehicles.van:
                return Van;

            case Vehicles.ambulance:
                return Ambulance;

        }

        throw new System.NotImplementedException();
    }

}

public enum Vehicles
{
    none,
    taxi,
    van,
    ambulance
}
