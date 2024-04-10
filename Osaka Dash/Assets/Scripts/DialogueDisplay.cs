using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TupleMath
{
    public static Tuple<int,int> addTuples(Tuple<int,int> a, Tuple<int,int> b)
    {
        return new Tuple<int, int>(a.Item1 + b.Item1, a.Item2 + b.Item2);
    }

    public static Tuple<int,int> modTuples(Tuple<int,int>a, Tuple<int,int> b)
    {
        return new Tuple<int, int>(a.Item1 % b.Item1, a.Item2 % b.Item2);
    }
}

public class DialogueDisplay : MonoBehaviour
{

    [Tooltip("Choice boxes should be in the list from left-to-right, then up-to-down")][SerializeField] List<DialogueQuestion> questionSelection;
    [SerializeField] int choiceGridSizeX,choiceGridSizeY;
    [SerializeField] bool wipeQuestionWhenShowingChoices;
    [Tooltip("The duration (in seconds) to wait between characters")][SerializeField] float textSpeed = 0.5f;
    string text = "";
    float textTimer;
    int textIndex;
    DialogueSystem caller;
    DialogueQuestion[,] choiceGrid;
    Tuple<int, int> selected;
    bool showQuestion;

    TextMeshProUGUI textbox;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        textbox = GetComponent<TextMeshProUGUI>();
        if(textbox == null) textbox = GetComponentInChildren<TextMeshProUGUI>();

        choiceGrid = new DialogueQuestion[choiceGridSizeY, choiceGridSizeX];

        for (int i = 0; i < questionSelection.Count; i++)
        {
            questionSelection[i].setParent(this);
            choiceGrid[i / choiceGridSizeX, i % choiceGridSizeX] = questionSelection[i];
        }
    }

    private void Update()
    {
        text = text.Replace("\\n", "\n");
        textTimer += Time.deltaTime;
        if (text == null || text.Length == 0) textbox.SetText("");
        else if (textTimer > textSpeed)
        {
            if(textIndex<text.Length) { textIndex++; }
            textbox.SetText(text.Substring(0, textIndex));
            textTimer = 0;
        }

        if (showQuestion)
        {
            /*Tuple<int, int> selectionMovement = new Tuple<int, int>(0, 0);
            if (Input.GetKeyDown(KeyCode.UpArrow)) selectionMovement = new Tuple<int, int>(choiceGridSizeY - 1, 0);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) selectionMovement = new Tuple<int, int>(1, 0);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) selectionMovement = new Tuple<int, int>(0, choiceGridSizeX - 1);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) selectionMovement = new Tuple<int, int>(0, 1);

            for (
                Tuple<int, int> nextSelection = TupleMath.modTuples(TupleMath.addTuples(selectionMovement, selected), new Tuple<int, int>(choiceGridSizeY, choiceGridSizeX));
                nextSelection != selected;
                nextSelection = TupleMath.modTuples(TupleMath.addTuples(selectionMovement, nextSelection), new Tuple<int, int>(choiceGridSizeY, choiceGridSizeX))
            )
                if (choiceGrid[nextSelection.Item1, nextSelection.Item2].setChoice())
                {
                    choiceGrid[selected.Item1, selected.Item2].deselect();
                    selected = nextSelection;
                }*/

            int flag =
                (Input.GetKeyDown(KeyCode.UpArrow) ? 8 : 0) |
                (Input.GetKeyDown(KeyCode.LeftArrow) ? 4 : 0) |
                (Input.GetKeyDown(KeyCode.DownArrow) ? 2 : 0) |
                (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0);
            if (flag!=0)
            {
                choiceGrid[selected.Item1, selected.Item2].deselect();
                for (
                    selected = new Tuple<int, int>(
                        (selected.Item1 + (((flag & 8) != 0) ? (choiceGridSizeY - 1) : ((flag & 2) != 0) ? 1 : 0)) % choiceGridSizeY,
                        (selected.Item2 + (((flag & 4) != 0) ? (choiceGridSizeX - 1) : ((flag & 1) != 0) ? 1 : 0)) % choiceGridSizeX);
                    !choiceGrid[selected.Item1, selected.Item2].setChoice();
                    selected = new Tuple<int, int>(
                        (selected.Item1 + (((flag & 8) != 0) ? (choiceGridSizeY - 1) : ((flag & 2) != 0) ? 1 : 0)) % choiceGridSizeY,
                        (selected.Item2 + (((flag & 4) != 0) ? (choiceGridSizeX - 1) : ((flag & 1) != 0) ? 1 : 0)) % choiceGridSizeX)
                ) ;
            }
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Wipe()
    {
        textIndex = 0;
        text = "";
    }

    public void addText(string text)
    {
        this.text += text;
    }

    public bool finishText()
    {
        if (text == null || text.Length == 0) return true;
        if (textIndex == text.Length) return true;
        textIndex = text.Length;
        return false;
    }

    public void displayQuestion(DialogueSystem caller,List<String> questionTexts)
    {
        if (wipeQuestionWhenShowingChoices)
            Wipe();
        this.caller = caller;
        for (int i = 0; i < questionTexts.Count && i < questionSelection.Count; i++)
        {
            questionSelection[i].displayChoice(questionTexts[i], i);
            questionSelection[i].deselect();
        }
        questionSelection[0].setChoice();
        selected = new Tuple<int, int>(0, 0);
        showQuestion = true;
    }

    public void endDialogue()
    {
        gameObject.SetActive(false);
    }

    public void AnswerGiven(int choice)
    {
        showQuestion = false;
        for (int i = 0; i < questionSelection.Count && i < questionSelection.Count; i++)
        {
            questionSelection[i].hideChoice();
            questionSelection[i].deselect();
        }
        Debug.Log(String.Format("answer {0} given", choice));
        caller.AnswerGiven(choice);
    }

    public void getChoice()
    {
        AnswerGiven(selected.Item1 * choiceGridSizeX + selected.Item2);
        return;
    }
}
