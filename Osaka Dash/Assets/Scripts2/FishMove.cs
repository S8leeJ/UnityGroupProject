using System.Collections;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public float speed;
    public float changeDirectionTime;
    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        changeDirectionTime = Random.Range(1f, 3f);
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());
        
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;

        // Rotate sprite based on movement direction
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            float moveHorizontal = Random.Range(-1f, 1f);
            float moveVertical = Random.Range(-1f, 1f);

            direction = new Vector2(moveHorizontal, moveVertical).normalized;

            yield return new WaitForSeconds(changeDirectionTime);
        }
    }
}


//In the sumi, incorporate best to my abilities (Top should say the name)
// 3 NPCs clueing the person where to go (with historical facts)
// finally leads to the pond where the koi and an old man are
// map of sumi
// old man explains the rules and then upon contact with the pond says that he needs help catching fish and starts the game
// at the end wins a goldfish trophy for sakekeeping in return
// if the 