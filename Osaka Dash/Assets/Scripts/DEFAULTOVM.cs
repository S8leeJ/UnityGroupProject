using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DEFAULTOVM : MonoBehaviour
{
    float horizontal, vertical;
    public GameObject aoi;
    bool frozen;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField][Tooltip("If this is 0 or less, it will use aoiCircle")] float aoiRadius;
    [SerializeField][Tooltip("This will use GetComponent<CircleCollider2D> if left empty")] CircleCollider2D aoiCircle;
    Animator animator, aoiAnim;
    [SerializeField] float[] facing = { 0f, 0f };
    // Start is called before the first frame update


    public AudioSource soundEffect;
    public int count = 0;
    public Text coins;

    void Start()
    {
        aoiAnim = aoi.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        frozen = false;
        if (aoiRadius <= 0)
        {
            if (aoiCircle == null) aoiRadius = GetComponent<CircleCollider2D>().radius;
            else aoiRadius = aoiCircle.radius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalEventSystem.isPaused())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (!frozen) rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) facing[0] = -1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) facing[0] = 1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow)) facing[1] = 1;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow)) facing[1] = -1;
        if (facing[0] == 1)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else transform.rotation = new Quaternion(0, 0, 0, 0);
        animator.SetInteger("HFacing", (int)facing[0]);
        animator.SetInteger("VFacing", (int)facing[1]);
        animator.SetInteger("HMoving", (int)(horizontal * 100));
        animator.SetInteger("VMoving", (int)(vertical * 100));

        Vector3 aoiMoveVector = transform.position - aoi.transform.position;
        float aoiDistance = aoiMoveVector.magnitude;
        aoiMoveVector.Normalize();
        aoiAnim.SetInteger("VMoving", (aoiMoveVector.y < -0.5f) ? -1 : (aoiMoveVector.y > 0.5f ? 1 : 0));
        aoiAnim.SetInteger("HMoving", Mathf.Abs(aoiMoveVector.x) > 0.5f ? 1 : 0);
        
        if (aoiMoveVector.x > 0.5f) aoi.transform.rotation = new Quaternion(0, 180, 0, 0);
        else aoi.transform.rotation = new Quaternion(0, 0, 0, 0);

        if (aoiDistance > aoiRadius)
        {
            aoi.transform.position += aoiMoveVector * speed * (aoiDistance / (2 * aoiRadius)) * Time.deltaTime;
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
        if (collision.gameObject.CompareTag("POI"))
        {
            count = 0;
            coins.text = "COINS: " + count.ToString() + "X";

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
            coins.text = "COINS: " + count.ToString() + " / 5X";


        }
    }
}
