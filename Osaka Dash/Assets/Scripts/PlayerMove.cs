using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed = 10, jump = 10;
    [SerializeField] int objectsLeft;
    float horizontal;
    private Vector2 ogPos;
    public GameObject overworldPlayer;
    public TextMeshProUGUI healthText;
    public int health = 3;
    // Start is called before the first frame update

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && Physics2D.Raycast(transform.position, Vector2.down, 1, LayerMask.GetMask("Ground")))
            rb.velocity = new Vector2(rb.velocity.x, jump);

        Quaternion rotation = transform.rotation;
        if (horizontal < 0) rotation.y = 0;
        else if (horizontal>0) rotation.y = 180;
        transform.rotation = rotation;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, transform.rotation.y == 0 ? Vector2.right : Vector2.left, 3f, LayerMask.GetMask("Ground"));
            if (hit.collider != null) {
                if (hit.collider.GetComponent<CrowdScript>() != null)
                {
                    hit.collider.GetComponent<CrowdScript>().Freeze();
                }
            }
        }

    }

    public void resetPos()
    {
        transform.position = ogPos;
    }

    public void setObjectsLeft(int obj)
    {
        objectsLeft = obj;
        health = 3;
        healthText.text = "Health = " + health;

    }

    public void setPos(Vector2 pos)
    {
        transform.position = pos;
    }

    public void setOgPos(Vector2 pos)
    {
        ogPos = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Collectible"))
        {
            collision.gameObject.SetActive(false);
            objectsLeft--;
            if (objectsLeft == 0)
            {
                overworldPlayer.GetComponent<OverworldMovement>().nextStage();
            }
        } else if (collision.gameObject.name.Contains("Crowd") || collision.gameObject.name.Contains("Crusher")
                   || collision.gameObject.name.Contains("Spike"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 15);
            resetPos();
            health--;
            if (health == 0)
            {
                overworldPlayer.GetComponent<OverworldMovement>().lostMinigame();
            }
            else healthText.text = "Health = " + health;
        }
    }
}
