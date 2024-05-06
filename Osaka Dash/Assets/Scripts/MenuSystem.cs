using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void TriggerMenuBoundDialogue(string dialogueName)
    {
        DialogueSystem.instance.TriggerDialogue(dialogueName);
    }
}
