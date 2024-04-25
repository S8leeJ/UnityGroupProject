using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMove : TriggerableEvent
{
    [SerializeField][Tooltip("This will be the acceleration if there is a RigidBody component")] Vector3 moveAmount;
    Vector3 alreadyMoved, velocity, originalPosition;
    [SerializeField] float moveDuration = 1;
    [SerializeField][Tooltip("This will not be used if there is no RigidBody component")] Vector3 startVelocity;

    Transform transform;
    Rigidbody2D rigidbody2D;
    Rigidbody rigidbody;

    bool flag = false, useAccel;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody = GetComponent<Rigidbody>();
        useAccel = !(rigidbody2D == null && rigidbody == null);
        velocity = useAccel ? (moveAmount / moveDuration) : startVelocity;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (useAccel)
        {
            if (!flag)
            {
                timer = 0;
                return;
            }
            timer -= Time.deltaTime;
            if (timer < moveDuration) flag = false;
            if (rigidbody == null) rigidbody2D.AddForce(moveAmount, ForceMode2D.Impulse);
            else rigidbody.AddForce(moveAmount, ForceMode.Impulse);
        }
        else
        {

            Vector3 thisFrameMovement = velocity * Time.deltaTime * (flag ? 1 : -1);
            if (thisFrameMovement.magnitude >= (flag ? moveAmount - alreadyMoved : alreadyMoved).magnitude)
            {
                alreadyMoved = flag ? moveAmount : alreadyMoved;
                //ReturnToDialogue();
            }
            else
            {
                alreadyMoved += thisFrameMovement * (flag ? 1 : -1);
            }
            transform.position = originalPosition + alreadyMoved;
        }
    }

    public override void Trigger()
    {
        flag = !flag;
        timer = moveDuration;
        if (useAccel)
        {
            if (rigidbody == null) rigidbody2D.velocity = velocity;
            else rigidbody.velocity = velocity;
        }
    }
}
