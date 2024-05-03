using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioSource soundEffect;

    private Vector2 moveDirection;
    private AnimationController animationController;
    private Rigidbody2D rb;
    private bool isJumping = false;
    public float jumpForce = 5f;
    public int count = 0;
    public Text coins; 
    void Start()
    {
        animationController = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(x, y).normalized;

        if (moveDirection != Vector2.zero)
        {
            animationController.ChangeMoveType((int)AnimationController.moveType.Walking);
            animationController.ChangeDirection((int)DetermineDirection(moveDirection));
        }
        else
        {
            animationController.ChangeMoveType((int)AnimationController.moveType.Idle);
        }
        coins.text = "COINS: " + count.ToString() + "X";

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    AnimationController.Direction DetermineDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        float angleInDegrees = angle * Mathf.Rad2Deg;

        if (angleInDegrees < 0)
        {
            angleInDegrees += 360;
        }

        if (angleInDegrees >= 45 && angleInDegrees < 135)
        {
            return AnimationController.Direction.BackRight;
        }
        else if (angleInDegrees >= 135 && angleInDegrees < 225)
        {
            return AnimationController.Direction.FrontRight;
        }
        else if (angleInDegrees >= 225 && angleInDegrees < 315)
        {
            return AnimationController.Direction.FrontRight;
        }
        else
        {
            return AnimationController.Direction.BackLeft;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalEventSystem.isInDialogue) return;
        // Check if the collided GameObject has the "NPC" tag
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPC npcScript = collision.gameObject.GetComponent<NPC>();

            // Check if the NPC script component exists
            if (npcScript != null)
            {
                // Call the Speak method on the NPC script component
                npcScript.DisplayDialog();
            }
        }
        if (collision.gameObject.CompareTag("CollectFish"))
        {
            Debug.Log("collide");
            Destroy(collision.gameObject);
            count++;
            soundEffect.Play();

        }
    }
}
