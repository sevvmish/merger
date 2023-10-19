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

    [SerializeField] private GameObject messageDel;
    [SerializeField] private TextMeshProUGUI messageDelText;

    [SerializeField] private GameObject messageUp;
    [SerializeField] private TextMeshProUGUI messageUpText;

    [SerializeField] private GameObject messageReplace;
    [SerializeField] private TextMeshProUGUI messageReplaceText;

    [SerializeField] private GameObject bonusReminder;
    [SerializeField] private GameObject bonusPanel;


    [SerializeField] private GameObject arrowExample;
    private List<GameObject> arrows = new List<GameObject> ();

    private bool isAct1Started;
    private bool isAct2Started;
    private bool isAct3Started;
    private bool isAct4Started;
    private bool isAct5Started;
    private bool isAct6Started;
    private bool isAct7Started;

    private bool isDel;
    private bool isRep;
    private bool isUp;

    private bool isFirstLine;
    private bool isSecondLine;
    private bool isThirdLine;

    private bool isReminded;

    private Translation lang;

    private List<Frame> frames = new List<Frame>();

    private float _timer = 2f;
    private float _timer2 = 0;

    private GameManager gm;

    // Start is called before the first frame update
    public void SetTutorial()
    {
        gm = GameManager.Instance;
        frames = GameManager.Instance.GetBaseFrames;
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        message1Text.text = lang.TutorialText1;
        message2Text.text = lang.TutorialText2;
        message3Text.text = lang.TutorialText3;
        message4Text.text = lang.TutorialText4;
        message5Text.text = lang.TutorialText5;
        message6Text.text = lang.TutorialText6;
        messageDelText.text = lang.TutorialTextDel;
        messageUpText.text = lang.TutorialTextUp;
        messageReplaceText.text = lang.TutorialTextReplace;

        message1.SetActive(false);
        message2.SetActive(false);
        message3.SetActive(false);
        message4.SetActive(false);
        message5.SetActive(false);
        message6.SetActive(false);
        messageDel.SetActive(false);
        messageUp.SetActive(false);
        messageReplace.SetActive(false);
        bonusReminder.SetActive(false);



    }

    private void DisableAllFrames()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            frames[i].SetStatusOnlyForTutorial(true);
        }
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

                for (int i = 0; i < 9; i++)
                {
                    GameObject g = Instantiate(arrowExample);
                    g.SetActive(false);
                    arrows.Add(g);
                }

                arrows[0].transform.position = frames[2].gameObject.transform.position;
                arrows[1].transform.position = frames[5].gameObject.transform.position;
                arrows[2].transform.position = frames[8].gameObject.transform.position;

                arrows[3].transform.position = frames[1].gameObject.transform.position;
                arrows[4].transform.position = frames[4].gameObject.transform.position;
                arrows[5].transform.position = frames[7].gameObject.transform.position;

                arrows[6].transform.position = frames[0].gameObject.transform.position;
                arrows[7].transform.position = frames[3].gameObject.transform.position;
                arrows[8].transform.position = frames[6].gameObject.transform.position;

            }

            if (isAct1Started && !message1.activeSelf && !isAct2Started)
            {
                if (!isFirstLine)
                {
                    isFirstLine = true;
                    arrows[0].SetActive(true);
                    arrows[1].SetActive(true);
                    arrows[2].SetActive(true);
                    frames[0].SetStatusOnlyForTutorial(true);
                    frames[1].SetStatusOnlyForTutorial(true);
                    frames[3].SetStatusOnlyForTutorial(true);
                    frames[4].SetStatusOnlyForTutorial(true);
                    frames[6].SetStatusOnlyForTutorial(true);
                    frames[7].SetStatusOnlyForTutorial(true);
                }
                else if (gm.Score>=3 && !isSecondLine)
                {
                    isSecondLine = true;

                    arrows[0].SetActive(false);
                    arrows[1].SetActive(false);
                    arrows[2].SetActive(false);

                    arrows[3].SetActive(true);
                    arrows[4].SetActive(true);
                    arrows[5].SetActive(true);

                    //DisableAllFrames();
                    frames[2].SetStatusOnlyForTutorial(true);
                    frames[5].SetStatusOnlyForTutorial(true);

                    frames[1].SetStatusOnlyForTutorial(false);
                    frames[4].SetStatusOnlyForTutorial(false);
                    frames[7].SetStatusOnlyForTutorial(false);
                }
                else if (gm.Score >= 6 && !isThirdLine)
                {
                    isThirdLine = true;

                    arrows[3].SetActive(false);
                    arrows[4].SetActive(false);
                    arrows[5].SetActive(false);

                    arrows[6].SetActive(true);
                    arrows[7].SetActive(true);
                    arrows[8].SetActive(true);

                    //DisableAllFrames();
                    frames[1].SetStatusOnlyForTutorial(true);
                    frames[4].SetStatusOnlyForTutorial(true);

                    frames[0].SetStatusOnlyForTutorial(false);
                    frames[3].SetStatusOnlyForTutorial(false);
                    frames[6].SetStatusOnlyForTutorial(false);
                }

                if (!isAct2Started && isThirdLine && gm.Score >= 9)
                {
                    isAct2Started = true;
                    message2.SetActive(true);
                }
                
            }

            if (isAct2Started && !isAct3Started && GameManager.Instance.Score >=9)
            {
                if (_timer2 > 1.5f)
                {
                    isAct3Started = true;
                    message3.SetActive(true);
                    //message2.SetActive(false);

                    arrows[6].SetActive(false);
                    arrows[7].SetActive(false);
                    arrows[8].SetActive(false);

                    DisableAllFrames();
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
                message2.SetActive(false);

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
                //GP_Analytics.Goal("tutorial", 0);
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

        if (Globals.CurrentLevel > 0 && Globals.CurrentLevel < 4 && GameManager.Instance.IsGameStarted)
        {
            //print(Globals.CurrentLevel + " = " + gm.CurrentGameEventToProceed);

            switch(Globals.CurrentLevel)
            {
                case 1:
                    if (gm.CurrentGameEventToProceed == GameEventsType.up_house )
                    {
                        if (!isUp)
                        {
                            isUp = true;
                            messageUp.SetActive(true);
                        }
                        
                    }
                    else
                    {
                        messageUp.SetActive(false);
                    }

                    if (gm.BonusProgress > 0.99f && !bonusPanel.activeSelf && !isReminded)
                    {
                        isReminded = true;
                        bonusReminder.SetActive(true);
                    }
                    else if (bonusPanel.activeSelf)
                    {
                        bonusReminder.SetActive(false);
                    }

                    break;

                case 2:
                    if (gm.CurrentGameEventToProceed == GameEventsType.delete_house)
                    {
                        if (!isDel)
                        {
                            isDel = true;
                            messageDel.SetActive(true);
                        }
                        
                    }
                    else
                    {
                        messageDel.SetActive(false);
                    }
                    break;

                case 3:
                    if (gm.CurrentGameEventToProceed == GameEventsType.replace_house)
                    {
                        if (!isRep)
                        {
                            isRep = true;
                            messageReplace.SetActive(true);
                        }
                        
                    }
                    else
                    {
                        messageReplace.SetActive(false);
                    }
                    break;
            }
        }
    }
}
