using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationAdvancer : TriggerableEvent
{
    int state;
    [SerializeField] string stageName;
    [SerializeField] int[] flipOn;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (flipOn.Contains(state)) GetComponent<Transform>().rotation = new Quaternion(0, 180, 0, 0);
        else GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
    }

    public override void Trigger()
    {
        animator.SetInteger(stageName, ++state);
        ReturnToDialogue();
    }
}
