using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public Animator anim;

    public enum moveType
    {
        Idle = 1,
        Walking,
        Jumping
    }

    //Clockwise counting starting from Front Left direction.
    public enum Direction
    {
        FrontLeft = 1,
        BackLeft,
        BackRight,
        FrontRight
    }

    //moveType moveT;
    //Direction dir;

    public void ChangeMoveType(int i)
    {
        anim.SetInteger("moveType", i);
    }

    public void ChangeDirection(int i)
    {
        anim.SetInteger("direction", i);
    }

}
