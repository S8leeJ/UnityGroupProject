using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{

    public float speed = 6f;
    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("changeDirection", 3.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        pos.y += speed * Time.deltaTime;
        transform.position = pos;
    }

    void changeDirection()
    {
        speed *= -1;
    }
}
