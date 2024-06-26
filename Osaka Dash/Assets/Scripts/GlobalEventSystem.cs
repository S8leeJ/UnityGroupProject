using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalEventSystem
{
    public static bool isInMinigame { get; private set; }
    public static bool isInDialogue { get; private set; }
    public static bool isInTransition { get; private set; }
    
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

    public static void SceneTransition(int sceneID)
    {
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }

    public static void SceneTransition(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}

public static class GlobalVariableSystem
{
    static Dictionary<string, int> integerList;
    static Dictionary<string, float> floatList;
    static Dictionary<string, bool> boolList;
}
