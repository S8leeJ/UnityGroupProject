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

    public void displayChoice(string text)
    {
        gameObject.SetActive (true);
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
}
