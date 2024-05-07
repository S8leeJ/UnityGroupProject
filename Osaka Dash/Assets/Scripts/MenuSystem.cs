using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
