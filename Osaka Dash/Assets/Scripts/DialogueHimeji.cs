using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHimeji : TriggerableEvent
{
    public GameObject player;

    // Start is called before the first frame update
    public void triggerDialogue()
    {
        GetComponent<DialogueSystem>().TriggerDialogue("c");
    }
    
    public override void Trigger()
    {
        player.GetComponent<OverworldMovement>().mg1();
        Debug.Log("successfully triggered event");
    }
}
