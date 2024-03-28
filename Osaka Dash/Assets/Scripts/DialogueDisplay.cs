using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [Tooltip("Choice boxes should be in the list from left-to-right, then up-to-down")][SerializeField] List<DialogueQuestion> questionSelection;
    [SerializeField] Tuple<int,int> choiceGridSize;
    [SerializeField] bool wipeQuestionWhenShowingChoices;
    [Tooltip("The duration (in seconds) to wait between characters")][SerializeField] const float textSpeed = 0.5f;
    string text;
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

        for(int i = 0; i < choiceGridSize.Item1; i++)
        {
            for (int j = 0; j < choiceGridSize.Item2; j++)
            {
                DialogueQuestion now = questionSelection[i * choiceGridSize.Item2 + j];
                if (choiceGridSize.Item2 != 1)
                {
                    if (j == choiceGridSize.Item2 - 1) now.setRight(questionSelection[(i - 1) * choiceGridSize.Item2 + j + 1]);
                    else now.setRight(questionSelection[i * choiceGridSize.Item2 + j + 1]);
                }
                if(choiceGridSize.Item1 != 1)
                {
                    if (i == choiceGridSize.Item1 - 1) now.setDown(questionSelection[j]);
                    else now.setDown(questionSelection[(i + 1) * choiceGridSize.Item2 + j]);
                }
            }
        }
    }

    private void Update()
    {
        if (textTimer > textSpeed)
        {
            if(textIndex<text.Length) { textIndex++; }
            textbox.SetText(text.Substring(0, textIndex));
        }
    }

    public void setText(string text)
    {
        textIndex = 0;
        this.text = text;
    }

    public void addText(string text)
    {
        this.text += text;
    }

    public bool finishText()
    {
        if (textIndex == text.Length) return true;
        textIndex = text.Length;
        return false;
    }

    public void displayQuestion(DialogueSystem caller,List<String> questionTexts)
    {
        if (wipeQuestionWhenShowingChoices)
            setText("");
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
