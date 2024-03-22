using JetBrains.Annotations;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AbstractDialogue
{
    public bool goToNext()
    {
        return true;
    }
}

public class DialogueLine : AbstractDialogue
{

}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField][Tooltip("relative path to the dialogue text file from StreamingAssets folder")] String relativePath;
    List<String> dialogues;
    [SerializeField] GameObject dialogueBox;

    void Awake()
    {
        if (relativePath.Length < 1) throw new Exception("no path to dialogue text file specified");
        else if (relativePath.Substring(0, "StreamingAssets".Length) == "StreamingAssets") relativePath = relativePath.Substring("StreamingAssets".Length);
        else if (relativePath[0] != '/') relativePath = '/' + relativePath;

        if (dialogueBox == null) throw new Exception("no dialogue box specified");

        dialogues = File.ReadAllLines(Application.streamingAssetsPath + relativePath).ToList();
    }
    
    void ImportDialogue(List<String> dialogues)
    {
        for(int i = 0; i < dialogues.Count; i++)
        {

        }
    }

    public void goNext()
    {
        
    }

    public void AnswerGiven(int answerChoice)
    {

    }
}
