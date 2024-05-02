using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWait : TriggerableEvent
{
    [SerializeField] float timerDuration = 1;
    float timer;
    bool triggered = false;

    private void Update()
    {
        if (!triggered) return;
        
        timer -= Time.deltaTime;
        if (timer < 0) {
            triggered = false;
            ReturnToDialogue();
        }
    }
    public override void Trigger()
    {
        timer = timerDuration;
        triggered = true;
    }
}