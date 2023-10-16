using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GamePush;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject message1;
    [SerializeField] private TextMeshProUGUI message1Text;
    [SerializeField] private GameObject message2;
    [SerializeField] private TextMeshProUGUI message2Text;
    [SerializeField] private GameObject message3;
    [SerializeField] private TextMeshProUGUI message3Text;
    [SerializeField] private GameObject message4;
    [SerializeField] private TextMeshProUGUI message4Text;
    [SerializeField] private GameObject message5;
    [SerializeField] private TextMeshProUGUI message5Text;
    [SerializeField] private GameObject message6;
    [SerializeField] private TextMeshProUGUI message6Text;


    [SerializeField] private GameObject arrowExample;
    private GameObject arrow1;
    private GameObject arrow2;
    private GameObject arrow3;

    private bool isAct1Started;
    private bool isAct2Started;
    private bool isAct3Started;
    private bool isAct4Started;
    private bool isAct5Started;
    private bool isAct6Started;
    private bool isAct7Started;

    private Translation lang;

    private List<Frame> frames = new List<Frame>();

    private float _timer = 2f;
    private float _timer2 = 0;



    // Start is called before the first frame update
    public void SetTutorial()
    {
        frames = GameManager.Instance.GetBaseFrames;
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        message1Text.text = lang.TutorialText1;
        message2Text.text = lang.TutorialText2;
        message3Text.text = lang.TutorialText3;
        message4Text.text = lang.TutorialText4;
        message5Text.text = lang.TutorialText5;
        message6Text.text = lang.TutorialText6;

        message1.SetActive(false);
        message2.SetActive(false);
        message3.SetActive(false);
        message4.SetActive(false);
        message5.SetActive(false);
        message6.SetActive(false);

        arrow1 = Instantiate(arrowExample);
        arrow2 = Instantiate(arrowExample);
        arrow3 = Instantiate(arrowExample);
        
        arrow1.SetActive(false);
        arrow2.SetActive(false);
        arrow3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;

        if (Globals.CurrentLevel == 0 && GameManager.Instance.IsGameStarted && _timer <= 0)
        {
            if (!isAct1Started)
            {
                frames = GameManager.Instance.GetBaseFrames;
                isAct1Started = true;
                message1.SetActive(true);

                arrow1.transform.position = frames[2].gameObject.transform.position;
                arrow2.transform.position = frames[5].gameObject.transform.position;
                arrow3.transform.position = frames[8].gameObject.transform.position;
                frames[0].SetStatusOnlyForTutorial(true);
                frames[1].SetStatusOnlyForTutorial(true);
                frames[3].SetStatusOnlyForTutorial(true);
                frames[4].SetStatusOnlyForTutorial(true);
                frames[6].SetStatusOnlyForTutorial(true);
                frames[7].SetStatusOnlyForTutorial(true);
            }

            if (isAct1Started && !message1.activeSelf && !isAct2Started)
            {
                
                arrow1.SetActive(true);
                arrow2.SetActive(true);
                arrow3.SetActive(true);
                

                isAct2Started = true;
                message2.SetActive(true);
            }

            if (isAct2Started && !isAct3Started && GameManager.Instance.Score >=3)
            {
                if (_timer2 > 1.5f)
                {
                    isAct3Started = true;
                    message3.SetActive(true);
                    message2.SetActive(false);

                    arrow1.SetActive(false);
                    arrow2.SetActive(false);
                    arrow3.SetActive(false);

                    frames[0].SetStatusOnlyForTutorial(false);
                    frames[1].SetStatusOnlyForTutorial(false);
                    frames[3].SetStatusOnlyForTutorial(false);
                    frames[4].SetStatusOnlyForTutorial(false);
                    frames[6].SetStatusOnlyForTutorial(false);
                    frames[7].SetStatusOnlyForTutorial(false);
                }
                else
                {
                    _timer2 += Time.deltaTime;
                }
                
            }

            if (isAct3Started && !message3.activeSelf && !isAct4Started)
            {
                isAct4Started = true;
                message4.SetActive(true);

            }

            if (isAct4Started && !message4.activeSelf && !isAct5Started)
            {
                isAct5Started = true;
                message5.SetActive(true);
            }

            if (isAct5Started && !message5.activeSelf && !isAct6Started)
            {
                isAct6Started = true;
                message6.SetActive(true);
            }

            if (isAct6Started && !message6.activeSelf && !isAct7Started)
            {
                isAct7Started = true;
                GP_Analytics.Goal("tutorial", 0);
                Globals.CurrentLevel++;
                if (Globals.MainPlayerData.Progress1 < Globals.CurrentLevel)
                {
                    Globals.MainPlayerData.Progress1 = Globals.CurrentLevel;
                    SaveLoadManager.Save();
                }
                GameManager.Instance.StopForTutorial();
                GameManager.Instance.StartSimpleGame();
            }
        }

        
    }
}
