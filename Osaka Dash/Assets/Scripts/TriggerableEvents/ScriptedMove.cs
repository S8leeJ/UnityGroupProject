using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMove : TriggerableEvent
{
    [SerializeField][Tooltip("This will be the acceleration if useAccel is checked")] Vector3 moveAmount;
    Vector3 velocity;
    [SerializeField] float moveDuration = 1;
    [SerializeField][Tooltip("This will not be used if useAccel is unchecked")] Vector3 startVelocity;

    new Transform transform;
    new Rigidbody2D rigidbody2D;
    new Rigidbody rigidbody;

    bool flag = false;
    [SerializeField] bool useAccel;
    [SerializeField][Tooltip("If this is unchecked, moveAmount will be treated as a global coordinate, only valid if useAccel is unchecked.")] bool useRelativeCoordinates = true;
    float timer = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag) return;

        timer += Time.deltaTime;
        if (timer > moveDuration) 
        {
            flag = false;
            ReturnToDialogue();
            return;
        }

        if (useAccel)
        {
            velocity += moveAmount * Time.deltaTime * 0.5f;
            if (rigidbody == null) rigidbody2D.velocity = velocity;
            else rigidbody.velocity = velocity;
            velocity += moveAmount * Time.deltaTime * 0.5f;
        }
        else
        {
            Vector3 thisFrameMovement = velocity * Time.deltaTime;
            transform.position += thisFrameMovement;
        }
    }

    public override void Trigger()
    {
        if (!useRelativeCoordinates) moveAmount -= transform.position;
        useAccel = useAccel && !(rigidbody2D == null && rigidbody == null);
        velocity = useAccel ? startVelocity : (moveAmount / moveDuration);
        flag = true;
    }
}
