using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("options menu")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private bool IsOptionsButtonShown;

    // Start is called before the first frame update
    void Start()
    {
        optionsButton.gameObject.SetActive(false);
        optionsPanel.SetActive(false);

        if (Globals.IsSoundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
        }

        SoundController _sound = SoundController.Instance;
        GameManager gm = GameManager.Instance;

        //options
        optionsButton.onClick.AddListener(() =>
        {
            _sound.PlayUISound(SoundsUI.click);
            optionsButton.gameObject.SetActive(false);
            optionsPanel.SetActive(true);
            

            continueButton.transform.localScale = Vector3.zero;
            soundButton.transform.localScale = Vector3.zero;
            homeButton.transform.localScale = Vector3.zero;

            continueButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            soundButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            homeButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        });

        continueButton.onClick.AddListener(() =>
        {
            _sound.PlayUISound(SoundsUI.click);
            optionsButton.gameObject.SetActive(true);
            optionsPanel.SetActive(false);
        });

        soundButton.onClick.AddListener(() =>
        {
            if (Globals.IsSoundOn)
            {
                Globals.IsSoundOn = false;
                soundButton.GetComponent<Image>().sprite = soundOffSprite;
                AudioListener.volume = 0;
            }
            else
            {
                _sound.PlayUISound(SoundsUI.click);
                Globals.IsSoundOn = true;
                soundButton.GetComponent<Image>().sprite = soundOnSprite;
                AudioListener.volume = 1f;
            }

            SaveLoadManager.Save();
        });

        homeButton.onClick.AddListener(() =>
        {
            
        });

        restartButton.onClick.AddListener(() =>
        {
            
        });
    }

    public void TurnAllOn()
    {
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(true);
    }

    public void TurnAllOff()
    {
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
    }


}
