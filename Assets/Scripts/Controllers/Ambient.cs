using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    private AssetManager assetManager;
    private AudioSource audioSource;

    public void SetData(AmbientType _type)
    {
        assetManager = GameManager.Instance.GetAssets;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        switch (_type)
        {
            case AmbientType.small_town:
                audioSource.clip = assetManager.SmallTownAmbient;
                audioSource.pitch = 0.5f;
                audioSource.volume = 0.7f;
                audioSource.loop = true;
                audioSource.Play();
                break;

            case AmbientType.forest:
                audioSource.clip = assetManager.ForestAmbient;
                audioSource.pitch = 0.5f;
                audioSource.volume = 0.7f;
                audioSource.loop = true;
                audioSource.Play();
                break;

            case AmbientType.desert:
                audioSource.clip = assetManager.DesertAmbient;
                audioSource.pitch = 0.9f;
                audioSource.volume = 0.5f;
                audioSource.loop = true;
                audioSource.Play();
                break;
        }
    }
}

public enum AmbientType
{
    none,
    small_town,
    forest,
    desert
}
