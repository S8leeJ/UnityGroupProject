using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHimeji : TriggerableEvent
{
    

    // Start is called before the first frame update
    public void triggerDialogue()
    {
        GetComponent<DialogueSystem>().TriggerDialogue("Questions Test");
    }
    
    public override void Trigger()
    {
        Debug.Log("successfully triggered event");
    }
}
