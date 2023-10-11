using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    private AssetManager assetManager;
    private AudioSource audioSource;
    private int rating;

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

    private void Update()
    {
        if (!audioSource.isPlaying && rating != 0) rating = 0;
    }

    public void PlayUISound(SoundsUI _type)
    {        
        switch(_type)
        {
            case SoundsUI.error:
                if (rating > 2) return;
                audioSource.Stop();
                audioSource.clip = assetManager.ErrorClip;
                audioSource.Play();
                rating = 2;
                break;

            case SoundsUI.positive:
                if (rating > 3) return;
                audioSource.Stop();
                audioSource.clip = assetManager.positiveSoundClip;
                audioSource.Play();
                rating = 3;
                break;

            case SoundsUI.error_big:
                audioSource.Stop();
                audioSource.clip = assetManager.ErrorBiggerClip;
                audioSource.Play();
                break;

            case SoundsUI.swallow:
                if (rating > 2) return;
                audioSource.Stop();
                audioSource.clip = assetManager.Swallow;
                audioSource.Play();
                rating = 2;
                break;

            case SoundsUI.tick:
                audioSource.Stop();
                audioSource.clip = assetManager.Tick;
                audioSource.Play();
                break;

            case SoundsUI.pop:
                if (rating > 1) return;
                audioSource.Stop();
                audioSource.clip = assetManager.Pop;
                audioSource.Play();
                rating = 1;
                break;

            case SoundsUI.click:
                audioSource.Stop();
                audioSource.clip = assetManager.Click;
                audioSource.Play();
                break;

            case SoundsUI.win:
                if (rating > 5) return;
                audioSource.Stop();
                audioSource.clip = assetManager.Win;
                audioSource.Play();
                rating = 5;
                break;

            case SoundsUI.lose:
                if (rating > 5) return;
                audioSource.Stop();
                audioSource.clip = assetManager.Lose;
                audioSource.Play();
                rating = 5;
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
    lose,
    bonus
}
