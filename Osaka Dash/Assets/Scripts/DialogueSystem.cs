using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChoice
{
    string text;
    Dialogue next;

    public AnswerChoice(string text, Dialogue next)
    {
        this.text = text;
        this.next = next;
    }

    public Dialogue getNext()
    {
        return next;
    }
}

public class DialogueLine
{
    bool isQuestion;
    string text;
    List<AnswerChoice> answerChoices;

    public DialogueLine(bool isQuestion, string text, string[] answers)
    {
        this.text = text;
        if (isQuestion == false) this.isQuestion = false;
        else
        {
            this.isQuestion = true;
            answerChoices = new List<AnswerChoice>();
            for (int i = 0; i < answers.Length; i++) 
            {
                //todo parsing answers format here
            }
        }
    }

    public bool isThisAQuestion()
    {
        return isQuestion;
    }

    public string toString()
    {
        return text;
    }
}

public class Dialogue
{
    static Dictionary<string,Dialogue> Dialogues = new Dictionary<string,Dialogue>();

    DialogueLine[] dialogueLines;

}

public class DialogueSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
