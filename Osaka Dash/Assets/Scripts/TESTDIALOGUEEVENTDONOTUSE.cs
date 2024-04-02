using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDIALOGUEEVENTDONOTUSE : TriggerableEvent
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DialogueSystem>().TriggerDialogue("Test Dialogue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Trigger()
    {
        Debug.Log("successfully triggered event");
    }
}
