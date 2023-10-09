using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    private AssetManager assetManager;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        assetManager = GameManager.Instance.GetAssets;
    }

    public void PlayUISound(SoundsUI _type)
    {
        switch(_type)
        {
            case SoundsUI.error:
                audioSource.Stop();
                audioSource.clip = assetManager.ErrorClip;
                audioSource.Play();
                break;

            case SoundsUI.positive:
                audioSource.Stop();
                audioSource.clip = assetManager.positiveSoundClip;
                audioSource.Play();
                break;

            case SoundsUI.error_big:
                audioSource.Stop();
                audioSource.clip = assetManager.ErrorBiggerClip;
                audioSource.Play();
                break;

            case SoundsUI.swallow:
                audioSource.Stop();
                audioSource.clip = assetManager.Swallow;
                audioSource.Play();
                break;

            case SoundsUI.tick:
                audioSource.Stop();
                audioSource.clip = assetManager.Tick;
                audioSource.Play();
                break;

            case SoundsUI.pop:
                audioSource.Stop();
                audioSource.clip = assetManager.Pop;
                audioSource.Play();
                break;

            case SoundsUI.click:
                audioSource.Stop();
                audioSource.clip = assetManager.Click;
                audioSource.Play();
                break;

            case SoundsUI.win:
                audioSource.Stop();
                audioSource.clip = assetManager.Win;
                audioSource.Play();
                break;

            case SoundsUI.lose:
                audioSource.Stop();
                audioSource.clip = assetManager.Lose;
                audioSource.Play();
                break;
        }
    }
}

public enum SoundsUI
{
    none,
    error,
    positive,
    error_big,
    swallow,
    tick,
    pop,
    click,
    win,
    lose
}
