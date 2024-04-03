using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [Tooltip("Choice boxes should be in the list from left-to-right, then up-to-down")][SerializeField] List<DialogueQuestion> questionSelection;
    [SerializeField] int choiceGridSizeX,choiceGridSizeY;
    [SerializeField] bool wipeQuestionWhenShowingChoices;
    [Tooltip("The duration (in seconds) to wait between characters")][SerializeField] float textSpeed = 0.5f;
    string text = "";
    float textTimer;
    int textIndex;
    DialogueSystem caller;

    TextMeshProUGUI textbox;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        textbox = GetComponent<TextMeshProUGUI>();
        if(textbox == null) textbox = GetComponentInChildren<TextMeshProUGUI>();

        for(int i = 0; i < choiceGridSizeX; i++)
        {
            for (int j = 0; j < choiceGridSizeY; j++)
            {
                if (questionSelection.Count < i * choiceGridSizeY + j)
                    break;
                DialogueQuestion now = questionSelection[i * choiceGridSizeY + j];
                if (choiceGridSizeY != 1)
                {
                    if (j == choiceGridSizeY - 1) now.setRight(questionSelection[(i - 1) * choiceGridSizeY + j + 1]);
                    else now.setRight(questionSelection[i * choiceGridSizeY + j + 1]);
                }
                if(choiceGridSizeX != 1)
                {
                    if (i == choiceGridSizeX - 1) now.setDown(questionSelection[j]);
                    else now.setDown(questionSelection[(i + 1) * choiceGridSizeY + j]);
                }
            }
        }
    }

    private void Update()
    {
        if (text == null || text.Length == 0) textbox.SetText("");
        else if (textTimer > textSpeed)
        {
            if(textIndex<text.Length) { textIndex++; }
            textbox.SetText(text.Substring(0, textIndex));
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Wipe()
    {
        textIndex = 0;
        this.text = "";
    }

    public void addText(string text)
    {
        this.text += text;
    }

    public bool finishText()
    {
        if (text == null || text.Length == 0) return true;
        if (textIndex == text.Length) return true;
        textIndex = text.Length;
        return false;
    }

    public void displayQuestion(DialogueSystem caller,List<String> questionTexts)
    {
        if (wipeQuestionWhenShowingChoices)
            Wipe();
        this.caller = caller;
        for (int i = 0; i < questionTexts.Count && i < questionSelection.Count; i++)
        {
            questionSelection[i].displayChoice(questionTexts[i], i);
        }
    }

    public void AnswerGiven(int choice)
    {
        caller.AnswerGiven(choice);
    }

}
