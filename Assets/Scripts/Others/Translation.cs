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

    public Translation() { }
}
