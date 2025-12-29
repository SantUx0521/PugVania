using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class Text : MonoBehaviour
{
    public LocalizedString[] localizedTexts;
    public int[] dialogueLengths; 
    public int dialogueStage = 0;
}
