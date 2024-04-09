using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 position = transform.position;

        // Check if moving left or right
        if (horizontal != 0)
        {
            position.x = position.x + 3.0f * horizontal * Time.deltaTime;
            if (horizontal < 0)
            {
                animator.SetInteger("Direction", 2); // Left
            }
            else
            {
                animator.SetInteger("Direction", 2); // Left
                transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);

            }
        }

        // Check if moving up or down
        if (vertical != 0)
        {
            position.y = position.y + 3.0f * vertical * Time.deltaTime;
            if (vertical < 0)
            {
                animator.SetInteger("Direction", 1); // Forward
            }
            else
            {
                animator.SetInteger("Direction", -1); // Backward
            }
        }

        transform.position = position;
    }
}
