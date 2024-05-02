using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMove : TriggerableEvent
{
    [SerializeField][Tooltip("This will be the acceleration if there is a RigidBody component")] Vector3 moveAmount;
    Vector3 velocity;
    [SerializeField] float moveDuration = 1;
    [SerializeField][Tooltip("This will not be used if there is no RigidBody component")] Vector3 startVelocity;

    Transform transform;
    Rigidbody2D rigidbody2D;
    Rigidbody rigidbody;

    bool flag = false, useAccel;
    float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody = GetComponent<Rigidbody>();
        useAccel = !(rigidbody2D == null && rigidbody == null);
        velocity = useAccel ? startVelocity : (moveAmount / moveDuration);
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
        flag = true;
    }
}
