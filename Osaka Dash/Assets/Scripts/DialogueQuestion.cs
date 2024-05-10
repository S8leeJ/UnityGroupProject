using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueQuestion : MonoBehaviour
{
    TextMeshProUGUI textbox;
    Button button;
    DialogueDisplay caller;
    public int choiceID;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        button = GetComponent<Button>();
        textbox = GetComponent<TextMeshProUGUI>();
        if (textbox == null)
            textbox = GetComponentInChildren<TextMeshProUGUI>();
        button.interactable = false;
    }

    public void displayChoice(string text)
    {
        gameObject.SetActive (true);
        textbox.SetText(text);
        button.interactable = true;
    }

    public void hideChoice()
    {
        gameObject.SetActive(false);
        button.interactable = false;
    }

    public bool setChoice()
    {
        if (!gameObject.activeSelf) return false;
        //change selection sound effect here
        button.Select();
        return true;
    }

    public void init(int id,DialogueDisplay parent)
    {
        choiceID = id;
        caller = parent;
    }

    public static int getCurrentChoice() => EventSystem.current.currentSelectedGameObject.GetComponent<DialogueQuestion>().choiceID;

    public void SendAnswer()
    {
        caller.AnswerGiven(choiceID);
    }
}
