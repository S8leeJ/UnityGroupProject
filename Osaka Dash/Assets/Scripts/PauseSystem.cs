using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    static bool isInMinigame, isInDialogue, isInTransition;
    
    public static bool isPaused()
    {
        return isInMinigame || isInDialogue || isInTransition;
    }

    public static void MinigameStart() { isInMinigame = true; }
    public static void MinigameEnd() { isInMinigame = false; }
    public static void DialogueStart() { isInDialogue = true; }
    public static void DialogueEnd() { isInDialogue = false; }
    public static void TransitionStart() { isInTransition = true; }
    public static void TransitionEnd() { isInTransition = false; }
}
