using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogueSystem.instance.TriggerDialogue("Opening");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
