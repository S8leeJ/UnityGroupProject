using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCharacters : TriggerableEvent
{
    [SerializeField] GameObject[] thingsToHide;

    public override void Trigger()
    {
        for (int i = 0; i < thingsToHide.Length; i++) 
        {
            thingsToHide[i].SetActive(false);
        }
    }
}
