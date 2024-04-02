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
        if (!isSelected) return;
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //select answer sound effect here
            Parent.AnswerGiven(choiceID);
        }
    }

    public void displayChoice(string text,int ID)
    {
        gameObject.SetActive (true);
        choiceID = ID;
        textbox.SetText(text);
    }

    public void setChoice()
    {
        //change selection sound effect here
        isSelected = true;
    }

    protected void setUp(DialogueQuestion question) { up = question; }
    public void setDown(DialogueQuestion question) { 
        down = question;
        down.setUp(this);
    }
    protected void setLeft(DialogueQuestion question) { left = question; }
    public void setRight(DialogueQuestion question) { 
        right = question;
        right.setLeft(this);
    }
    public void goUp()
    {
        if (up == null) return;
        isSelected = false;
        up.setChoice();
        if (up.gameObject.activeSelf == false) up.goUp();
        return;
    }
    public void goDown()
    {
        if (down == null) return;

        isSelected = false;
        down.setChoice();
        if (down.gameObject.activeSelf == false) down.goDown();
        return;
    }
    public void goLeft()
    {
        if (left == null) return;
        isSelected = false;
        left.setChoice();
        if (left.gameObject.activeSelf == false) left.goLeft();
        return;
    }
    public void goRight()
    {
        if (right == null) return;
        isSelected = false;
        right.setChoice();
        if (right.gameObject.activeSelf == false) right.goRight();
        return;
    }
}
