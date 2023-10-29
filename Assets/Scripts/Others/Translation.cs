using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    public string PlayText;
    public string BonusText;
    public string CustomGameText;    

    public string WinText;
    public string LoseText;
    public string LevelText;
    public string TutorialText;
    public string SkipText;

    public string TutorialText1;
    public string TutorialText2;
    public string TutorialText3;
    public string TutorialText4;
    public string TutorialText5;
    public string TutorialText6;
    public string TutorialText7;

    public string TutorialTextDel;
    public string TutorialTextUp;
    public string TutorialTextReplace;

    public string ResetText;

    public Translation() { }
}
