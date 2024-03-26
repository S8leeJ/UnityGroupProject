using JetBrains.Annotations;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class TriggerableEvent : MonoBehaviour
{
    public abstract void Trigger();
}

public abstract class AbstractDialogue
{
    public virtual bool goToNext()
    {
        return true;
    }

    public virtual string getText() { return ""; }
    public virtual List<string> getQuestionChoices() { return null; }
    public abstract int getType();
    public virtual TriggerableEvent getEvent() { return null; }
    //0 = regular dialogue line, 1 = add on, 2 = question, 3 = go to specific index, 4 = trigger event
    public virtual int getChoiceGoTo(int i) { return -1; }

}

public class DialogueLine : AbstractDialogue
{
    string text;
    bool addOn;
    public override string getText() { return text; }
    public override int getType() { return addOn ? 0 : 1; }
    public DialogueLine(string line) { text = line; }
    public DialogueLine(string line, bool addOn) { text = line; this.addOn = addOn; }
}

public class Question : AbstractDialogue
{
    string question;
    List<string> answers;
    List<int> choiceGoTo;
    public override string getText() { return question; }
    public override List<string> getQuestionChoices() { return answers; }
    public override int getType() { return 2; }
    public override int getChoiceGoTo(int i)
    {
        return choiceGoTo[i];
    }
    public Question(string question, List<string> answers,List<int> choiceEffects)
    {
        this.question = question;
        this.answers = answers;
        choiceGoTo = choiceEffects;
    }
}

public class DialogueEvent : AbstractDialogue
{
    TriggerableEvent triggerableEvent;
    public override int getType() { return 4; }
    public override TriggerableEvent getEvent()
    {
        return triggerableEvent;
    }
    public DialogueEvent(TriggerableEvent triggerableEvent)
    {
        this.triggerableEvent = triggerableEvent;
    }
}

public class DialogueGoTo : AbstractDialogue
{
    bool endDialogue;
    int indexToGo;

    public override string getText()
    {
        return endDialogue ? "1" + indexToGo : "0" + indexToGo;
    }
    public override int getType() { return 3; }

    public DialogueGoTo(bool endDialogue, int indexToGo)
    {
        this.endDialogue = endDialogue;
        this.indexToGo = indexToGo;
    }
}

public class Dialogue
{
    int dialoguePointer;
    List<AbstractDialogue> dialogueList;

    public void addDialogueLine(string text)
    {
        dialogueList.Add(new DialogueLine(text));
    }

    public void addQuestion(string question,List<string> answers,List<int> choiceEffects)
    {
        dialogueList.Add(new Question(question, answers, choiceEffects));
    }

    public void addEvent(TriggerableEvent Event){
        dialogueList.Add(new DialogueEvent(Event));
    }

    public void addGoTo(bool endDialogue, int indexToGo)
    {
        dialogueList.Add(new DialogueGoTo(endDialogue, indexToGo));
    }

    public AbstractDialogue Advance(int i)
    {
        dialoguePointer += i;
        for (; dialoguePointer >= dialogueList.Count; dialoguePointer -= dialogueList.Count) ;
        for (; dialoguePointer < 0; dialoguePointer += dialogueList.Count) ;
        return dialogueList[++dialoguePointer];
    }
    public AbstractDialogue Advance()
    {
        return Advance(1);
    }

    public AbstractDialogue Now()
    {
        return dialogueList[dialoguePointer];
    }

    public AbstractDialogue Go(int i)
    {
        if (i >= dialogueList.Count || i < 0) return Now();
        dialoguePointer = i;
        return dialogueList[dialoguePointer];
    }

    public Dialogue()
    {
        dialoguePointer = 0;
        dialogueList = new List<AbstractDialogue>();
    }

    //constructing methods, only to be used while parsing the Dialogue.

}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField][Tooltip("relative path to the dialogue text file from StreamingAssets folder")] String relativePath;
    [SerializeField] GameObject dialogueBox;
    Dictionary<string, Dialogue> dialogueDict;

    void Awake()
    {
        if (relativePath.Length < 1) throw new Exception("no path to dialogue text file specified");
        else if (relativePath.Substring(0, "StreamingAssets".Length) == "StreamingAssets") relativePath = relativePath.Substring("StreamingAssets".Length);
        else if (relativePath[0] != '/') relativePath = '/' + relativePath;

        if (dialogueBox == null) throw new Exception("no dialogue box specified");

        ImportDialogue(File.ReadAllLines(Application.streamingAssetsPath + relativePath).ToList());
    }
    
    void ImportDialogue(List<String> dialogues)
    {
        Dialogue currentDialogue = null;
        for(int i = 0; i < dialogues.Count; i++)
        {
            if (string.IsNullOrEmpty(dialogues[i])) continue;
            char firstLetter = dialogues[i][0];
            char secondLetter = dialogues[i][1];
            string content = dialogues[i].Substring(2);
            if(firstLetter!='='&&currentDialogue == null) { throw new Exception("No dialogue name specified"); }
            switch(firstLetter)
            {
                case '=':
                    currentDialogue = new Dialogue();
                    dialogueDict.Add(content, currentDialogue);
                    break;
                case 'Q':
                case 'q':

            }
        }
    }

    public void goNext()
    {
        
    }

    public void AnswerGiven(int answerChoice)
    {

    }
}
