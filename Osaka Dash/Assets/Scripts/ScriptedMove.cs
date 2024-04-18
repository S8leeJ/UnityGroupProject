using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMove : TriggerableEvent
{
    [SerializeField] Vector3 moveAmount;
    Vector3 alreadyMoved, velocity;
    [SerializeField] float moveDuration = 1;

    Transform transform;

    bool flag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        velocity = moveAmount / moveDuration;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 thisFrameMovement = velocity * Time.deltaTime * (flag ? 1 : -1);
        if (thisFrameMovement.magnitude >= ((flag?moveAmount:Vector3.zero)-alreadyMoved).magnitude)
        {

        }
    }

    public override void Trigger()
    {
        flag = !flag;
    }
}
