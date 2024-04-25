using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTesting : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Home))
        {
            DialogueSystem.instance.TriggerDialogue("Train Depart");
        }
    }
}