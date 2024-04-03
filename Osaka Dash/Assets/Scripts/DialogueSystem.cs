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
    //0 = regular dialogue line, 1 = add on, 2 = question, 3 = go to specific index or jump, 4 = trigger event, 5 = scene transition
    public virtual int getChoiceGoTo(int i) { return -1; }

}

public class DialogueLine : AbstractDialogue
{
    string text;
    bool addOn;
    public override string getText() { return text; }
    public override int getType() { return addOn ? 0 : 1; }
    public DialogueLine(string line) { text = line.Replace("\\n", "\n"); }
    public DialogueLine(string line, bool addOn) { text = line; this.addOn = addOn; }
}

public class Question : AbstractDialogue
{
    string question;
    List<Tuple<string,int>> answers;
    public override string getText() { return question; }
    public override List<string> getQuestionChoices()
    {
        List<string> answerText = new List<string>();
        for(int i = 0; i < answers.Count; i++)
        {
            answerText.Add(answers[i].Item1);
        }
        return answerText;
    }
    public override int getType() { return 2; }
    public override int getChoiceGoTo(int i)
    {
        return answers[i].Item2;
    }
    public Question(string question)
    {
        this.question = question;
        answers = new List<Tuple<string, int>>();
    }

    public void addAnswer(string answer,int done)
    {
        answers.Add(new Tuple<string, int>(answer, done));
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
    bool isRelative;
    int indexToGo;

    public override string getText()
    {
        return ((isRelative ? 1 : 0) + (endDialogue ? 2 : 0)).ToString() + indexToGo;
    }
    public override int getType() { return 3; }

    public DialogueGoTo(bool endDialogue, int indexToGo, bool isRelative)
    {
        this.endDialogue = endDialogue;
        this.indexToGo = indexToGo;
        this.isRelative = isRelative;
    }
}

public class DialogueSceneTransition : AbstractDialogue
{
    string sceneName;
    public override string getText() { return sceneName; }
    public override int getType() { return 5; }

    public DialogueSceneTransition(string sceneName)
    {
        this.sceneName = sceneName;
    }
}

public class Dialogue
{
    int dialoguePointer;
    List<AbstractDialogue> dialogueList;

    public AbstractDialogue Advance(int i)
    {
        dialoguePointer += i;
        for (; dialoguePointer < 0; dialoguePointer += dialogueList.Count) ;
        dialoguePointer %= dialogueList.Count;
        return dialogueList[dialoguePointer];
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

    public Dialogue(List<AbstractDialogue> list)
    {
        dialoguePointer = 0;
        dialogueList = list;
    }
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField][Tooltip("relative path to the dialogue text file from StreamingAssets folder")] String relativePath;
    [SerializeField] DialogueDisplay dialogueBox;
    [SerializeField] List<TriggerableEvent> eventList;
    Dictionary<string, Dialogue> dialogueDict;
    Dialogue nowDialogue;

    public static DialogueSystem instance { get; private set; }

    void Awake()
    {
        if (relativePath.Length < 1) throw new Exception("no path to dialogue text file specified");
        else if (relativePath.Substring(0, "StreamingAssets".Length) == "StreamingAssets") relativePath = relativePath.Substring("StreamingAssets".Length);
        else if (relativePath[0] != '/') relativePath = '/' + relativePath;

        if (dialogueBox == null) throw new Exception("no dialogue box specified");

        dialogueDict = new Dictionary<string, Dialogue>();

        ImportDialogue(File.ReadAllLines(Application.streamingAssetsPath + relativePath).ToList());
    }

    AbstractDialogue parseDialogue(char type, string content)
    {
        switch (type)
        {
            case '-':
                return new DialogueLine(content, false);
            case '+':
                return new DialogueLine(content, true);
            case 'G':
            case 'g':
            case 'J':
            case 'j':
                int pointer;
                if (!int.TryParse(content, out pointer))
                    throw new Exception("G, g, J, or j command used without valid dialogue index.");
                return new DialogueGoTo("GJ".IndexOf(type) < 0, pointer, "Gg".IndexOf(type) < 0);
            case 'S':
            case 's':
                return new DialogueSceneTransition(content);
            case 'T':
            case 't':
                int index;
                if (!int.TryParse(content, out index))
                    throw new Exception("T command used without valid event index");
                return new DialogueEvent(eventList[index]);
            default:
                Debug.Log(String.Format("Improper dialogue type {1} in {0}, this was treated as regular dialogue.\nContent of this dialogue was {2}.", relativePath, type, content));
                goto case '-';
        }
    }
    void ImportDialogue(List<String> dialogues)
    {
        List<AbstractDialogue> currentDialogue = new List<AbstractDialogue>();
        for(int i = 0; i < dialogues.Count; i++)
        {
            if (string.IsNullOrEmpty(dialogues[i])) continue;
            char firstLetter = dialogues[i][0];
            string content = dialogues[i].Substring(2);
            if(firstLetter!='='&&currentDialogue == null) { throw new Exception("No dialogue name specified"); }
            switch(firstLetter)
            {
                case '=':
                    currentDialogue.Add(new DialogueGoTo(true, 0, false));
                    dialogueDict.Add(content, new Dialogue(currentDialogue));
                    currentDialogue = new List<AbstractDialogue>();
                    break;
                case 'Q':
                case 'q':
                    Question now = new Question(content);
                    currentDialogue.Add(now);
                    int answersSize = 1;
                    for (; answersSize + i + 1 < dialogues.Count; answersSize++)
                    {
                        if ("Aa".IndexOf(dialogues[answersSize + i][0])<0)
                            break;
                    }
                    for(int j = 1; j <= answersSize; j++)
                    {
                        if (dialogues[i + j][0] == 'A')
                        {
                            now.addAnswer(dialogues[i + j].Substring(2), j);
                            if (j != 1)
                                currentDialogue.Add(new DialogueGoTo(false, answersSize - j, true));
                        }
                        else if (dialogues[i + j][0] == 'a')
                            currentDialogue.Add(parseDialogue(dialogues[i + j][1], dialogues[i + j].Substring(2)));
                        else
                            break;
                    }
                    i += answersSize - 1;
                    break;
                default:
                    currentDialogue.Add(parseDialogue(firstLetter, content));
                    break;
            }
        }
    }

    void Update()
    {
        if (!GlobalEventSystem.isInDialogue) return;

        AbstractDialogue now = nowDialogue.Now();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogueBox.finishText())
            {
                switch (now.getType())
                {
                    case 0:
                        dialogueBox.Wipe();
                        goto case 1;
                    case 1:
                        dialogueBox.addText(now.getText());
                        nowDialogue.Advance();
                        break;
                    case 2:
                        dialogueBox.Wipe();
                        dialogueBox.addText(now.getText());
                        break;
                    case 3:
                        string temp = now.getText();
                        if (temp.Length == 0) throw new SystemException("wtf");
                        switch (temp[0])
                        {
                            case '0':
                                nowDialogue.Go(int.Parse(temp.Substring(1)));
                                break;
                            case '1':
                                nowDialogue.Advance(int.Parse(temp.Substring(1)));
                                break;
                            case '2':
                                nowDialogue.Go(int.Parse(temp.Substring(1)));
                                goto default;
                            case '3':
                                nowDialogue.Advance(int.Parse(temp.Substring(1)));
                                goto default;
                            default:
                                GlobalEventSystem.DialogueEnd();
                                break;
                        }
                        break;
                    case 4:
                        now.getEvent().Trigger();
                        break;
                    case 5:
                        int tempID;
                        if (int.TryParse(now.getText(), out tempID))
                            GlobalEventSystem.SceneTransition(tempID);
                        else
                            GlobalEventSystem.SceneTransition(now.getText());
                        break;
                }
            }
        }
    }

    public void TriggerDialogue(string name)
    {
        if (!dialogueDict.ContainsKey(name))
            throw new System.Exception(string.Format("Invalid dialogue name: {0}", name));
        GlobalEventSystem.DialogueStart();
        dialogueBox.Enable();
        nowDialogue = dialogueDict[name];
    }

    public void AnswerGiven(int answerChoice)
    {
        nowDialogue.Advance(nowDialogue.Now().getChoiceGoTo(answerChoice));
    }
}
