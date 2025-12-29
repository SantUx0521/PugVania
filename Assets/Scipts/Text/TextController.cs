using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private Animator anim;
    private Queue<String> dialogueTail;
    Text texto;
    [SerializeField] TextMeshProUGUI screenText;
    public InteractObject interactable;
    [SerializeField] private LockedDoor lockedDoor;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject desitionBox;
    private PlayerStateList pState;
    public bool isDesitionBoxActive = false;
    private bool isTyping = false;
    private string currentFullText = "";
    public bool isDialogueActive = false;
    private Coroutine typingCoroutine;
    private bool isCinematic = false;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] int charToSound;

    
    private void Awake()
    {
        dialogueTail = new Queue<string>();
        dialogueBox.SetActive(false);
    }
    private void Update()
    {
        if (isDialogueActive && !isCinematic && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                screenText.text = currentFullText;
                isTyping = false;
            }
            else
            {
                nextPhrase();
            }
        }
    }

    public void StartDialogueFromCinematic()
    {
        isCinematic = true;
        dialogueBox.SetActive(true);
        ActiveCartel(interactable.textData);
    }
    public void ActiveCartel(Text textObject)
    {
        dialogueBox.SetActive(true);
        MovementPlayer.Instance.pState.canMove = false;
        texto = textObject;
        ActiveText();
    }

    public async void ActiveText()
    {   
        isDialogueActive = true;
        dialogueTail.Clear();

        int startIndex = 0;

        for (int i = 0; i < texto.dialogueStage && i < texto.dialogueLengths.Length; i++)
        {
            startIndex += texto.dialogueLengths[i];
        }
        int count = texto.dialogueStage < texto.dialogueLengths.Length
            ? texto.dialogueLengths[texto.dialogueStage]
            : texto.dialogueLengths[texto.dialogueLengths.Length - 1];

        for (int i = startIndex; i < startIndex + count && i < texto.localizedTexts.Length; i++)
        {
            var localizedText = texto.localizedTexts[i];
            string result = await localizedText.GetLocalizedStringAsync().Task;
            dialogueTail.Enqueue(result);
        }

        nextPhrase();

        if (texto.dialogueStage < texto.dialogueLengths.Length - 1)
        {
            texto.dialogueStage++;
        }
    }

    public void nextPhrase()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            screenText.text = currentFullText;
            isTyping = false;
            return;
        }
        if (dialogueTail.Count == 0)
        {
            if (isDesitionBoxActive)
            {
                ShowDecisionBox();
            }
            else
            {
                CloseCartel();
            }
            return;
        }
        currentFullText = dialogueTail.Dequeue();
        typingCoroutine = StartCoroutine(ShowCharacter(currentFullText));
    }

    public void ShowDecisionBox()
    {
        desitionBox.SetActive(true);
    }

    IEnumerator ShowCharacter(string showText)
    {
        isTyping = true;
        screenText.text = "";
        int charIndex = 0;

        foreach (char caracter in showText.ToCharArray())
        {
            screenText.text += caracter;
            if (charIndex % charToSound == 0)
            {
                audioSource.PlayOneShot(audioClip);
            }
            charIndex++;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        if (isCinematic)
        {
            yield return new WaitForSeconds(5f);
            if (isCinematic && isDialogueActive)
            {
                nextPhrase();
            }
        }   
    }

    public void CloseCartel()
    {
        dialogueBox.SetActive(false);
        screenText.text = "";
        MovementPlayer.Instance.pState.canMove = true;
        isDialogueActive = false;
    }
}
