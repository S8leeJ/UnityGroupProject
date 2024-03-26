using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 2.2f;
    private Vector2 ogPos;
    bool frozen = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!frozen) rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Destroy(collision.gameObject);
        }
        else speed *= -1;
    }

    public void resetPos()
    {
        transform.position = ogPos;
    }

    public void Freeze()
    {
        StartCoroutine(freeze());
    }
    IEnumerator freeze()
    {
        frozen = true;
        yield return new WaitForSeconds(3.0f);
        frozen = false;
    }
}
