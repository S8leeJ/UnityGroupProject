using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class DialogueQuestion : MonoBehaviour
{
    TextMeshProUGUI textbox;
    [SerializeField] GameObject selectedGraphic;
    bool isSelected = false;
    DialogueQuestion left, right, up, down;
    DialogueDisplay Parent;
    int choiceID;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        textbox=GetComponent<TextMeshProUGUI>();
        if (textbox == null)
            textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        selectedGraphic.SetActive(isSelected);
    }

    public void displayChoice(string text,int ID)
    {
        gameObject.SetActive (true);
        choiceID = ID;
        textbox.SetText(text);
    }

    public void hideChoice()
    {
        gameObject.SetActive(false);
        isSelected = false;
    }

    public bool setChoice()
    {
        if (!gameObject.activeSelf) return false;
        //change selection sound effect here
        isSelected = true;
        return true;
    }

    public void deselect()
    {
        isSelected = false;
    }

    public void setParent(DialogueDisplay parent)
    {
        Parent = parent;
    }
}
