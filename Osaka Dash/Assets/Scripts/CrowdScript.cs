using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 4.3f;
    private Vector2 ogPos;
    public bool frozen = false;
    // Start is called before the first frame update
    void Awake()
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
        speed *= -1;
        if (speed == 4.3f) transform.rotation = new Quaternion(0, 180, 0, 0);
        else transform.rotation = new Quaternion(0, 0, 0, 0);
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
