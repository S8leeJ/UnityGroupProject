using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TupleMath
{
    public static Tuple<int,int> addTuples(Tuple<int,int> a, Tuple<int,int> b)
    {
        return new Tuple<int, int>(a.Item1 + b.Item1, a.Item2 + b.Item2);
    }

    public static Tuple<int,int> modTuples(Tuple<int,int>a, Tuple<int,int> b)
    {
        return new Tuple<int, int>(a.Item1 % b.Item1, a.Item2 % b.Item2);
    }
}

public class DialogueDisplay : MonoBehaviour
{

    [Tooltip("Choice boxes should be in the list from left-to-right, then up-to-down")][SerializeField] List<DialogueQuestion> questionSelection;
    [SerializeField] bool wipeQuestionWhenShowingChoices;
    [Tooltip("The duration (in seconds) to wait between characters")][SerializeField] float textSpeed = 0.5f;
    string text = "";
    float textTimer;
    int textIndex;
    DialogueSystem caller;
    bool showQuestion;

    TextMeshProUGUI textbox;
    
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        textbox = GetComponent<TextMeshProUGUI>();
        if(textbox == null) textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        text = text.Replace("\\n", "\n");
        textTimer += Time.deltaTime;
        if (text == null || text.Length == 0) textbox.SetText("");
        else if (textTimer > textSpeed)
        {
            if(textIndex<text.Length) { textIndex++; }
            textbox.SetText(text.Substring(0, textIndex));
            textTimer = 0;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Wipe()
    {
        textIndex = 0;
        text = "";
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
            questionSelection[i].displayChoice(questionTexts[i]);
            questionSelection[i].init(i, this);
        }
        questionSelection[0].setChoice();
        showQuestion = true;
    }

    public void endDialogue()
    {
        gameObject.SetActive(false);
    }

    public void AnswerGiven(int choice)
    {
        showQuestion = false;
        for (int i = 0; i < questionSelection.Count && i < questionSelection.Count; i++)
        {
            questionSelection[i].hideChoice();
        }
        Debug.Log(String.Format("answer {0} given", choice));
        caller.AnswerGiven(choice);
    }

    public void getChoice()
    {
        AnswerGiven(DialogueQuestion.getCurrentChoice());
        return;
    }
}
