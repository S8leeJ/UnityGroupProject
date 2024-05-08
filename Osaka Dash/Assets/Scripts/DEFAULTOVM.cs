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
    public float speed;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalEventSystem.isPaused()) { return; }
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

        if (!aoi.GetComponent<BoxCollider2D>().IsTouching(GetComponent<CircleCollider2D>()))
        {
            if ((int)aoi.transform.position.y < (int)transform.position.y)
            {

                // use Vector2.moveTowards()
                //aoi.transform.position = new Vector2(aoi.transform.position.x, aoi.transform.position.y + (6.8f * Time.deltaTime));
                //aoi.transform.position = Vector2.MoveTowards(aoi.transform.position, transform.position, 6.8f * Time.deltaTime);
                aoiAnim.SetInteger("VMoving", 1);
            }
            else if ((int)aoi.transform.position.y > (int)transform.position.y)
            {
                //aoi.transform.position = new Vector2(aoi.transform.position.x, aoi.transform.position.y - (6.8f * Time.deltaTime));
                aoiAnim.SetInteger("VMoving", -1);
            }
            else
            {
                aoiAnim.SetInteger("VMoving", 0);
            }

            if ((int)aoi.transform.position.x < (int)transform.position.x)
            {
                aoi.transform.rotation = new Quaternion(0, 180, 0, 0);
                //aoi.transform.position = new Vector2(aoi.transform.position.x + (6.8f * Time.deltaTime), aoi.transform.position.y);
                aoiAnim.SetInteger("HMoving", 1);
            }
            else if ((int)aoi.transform.position.x > (int)transform.position.x)
            {
                aoi.transform.rotation = new Quaternion(0, 0, 0, 0);
                //aoi.transform.position = new Vector2(aoi.transform.position.x - (6.8f * Time.deltaTime), aoi.transform.position.y);
                aoiAnim.SetInteger("HMoving", -1);
            }
            else
            {
                aoiAnim.SetInteger("HMoving", 0);
            }
            aoi.transform.position = Vector2.MoveTowards(aoi.transform.position, transform.position, 6.8f * Time.deltaTime);
        }
        else
        {
            aoiAnim.SetInteger("VMoving", 0);
            aoiAnim.SetInteger("HMoving", 0);
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

        }
    }
}
