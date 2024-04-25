using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWait : TriggerableEvent
{
    [SerializeField] float timerDuration = 1;
    float timer;

    private void Update()
    {
        if(timer > 0) timer -= Time.deltaTime;
        else if (timer < 0) { 
            timer = 0;
            ReturnToDialogue();
        }
    }
    public override void Trigger()
    {
        timer = timerDuration;
    }
}