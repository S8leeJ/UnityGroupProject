using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHimeji : TriggerableEvent
{
    public GameObject player;

    // Start is called before the first frame update
    public void triggerDialogue(string d)
    {
        GetComponent<DialogueSystem>().TriggerDialogue(d);
    }
    
    public override void Trigger()
    {
        player.GetComponent<OverworldMovement>().minigame();
        Debug.Log("successfully triggered event");
    }
}
