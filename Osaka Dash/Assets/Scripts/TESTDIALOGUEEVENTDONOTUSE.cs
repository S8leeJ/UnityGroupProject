using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDIALOGUEEVENTDONOTUSE : TriggerableEvent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalEventSystem.isPaused()) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<DialogueSystem>().TriggerDialogue("Questions Test");
        }
    }

    public override void Trigger()
    {
        Debug.Log("successfully triggered event");
    }
}
