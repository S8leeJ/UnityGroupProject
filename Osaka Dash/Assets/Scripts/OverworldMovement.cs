using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    float horizontal, vertical;
    Rigidbody2D rb;
    public CinemachineVirtualCamera cam;
    public GameObject minigamePlayer, aoi;
    public GameObject[] stage1objs;
    public GameObject[] minigame1objs;
    public GameObject[] stage2objs;
    public GameObject[] minigame2objs;
    public GameObject[] stage3objs;
    public GameObject[] minigame3objs;
    public GameObject[] stage4objs;
    public DialogueHimeji dialogue;
    Animator animator, aoiAnim;
    public string objects;        // player may collect objects, like the himeji ticket. if so, store here to trigger events.
    bool frozen;
    public int stage;
    [SerializeField] float[] facing = { 0f, 0f };
    // Start is called before the first frame update
    void Start()
    {
        // 105, 31
        animator = GetComponent<Animator>();
        aoiAnim = aoi.GetComponent<Animator>();
        frozen = false;
        stage = 1;
        rb = GetComponent<Rigidbody2D>();
        dialogue = GetComponent<DialogueHimeji>();
        minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
    }

    public int getStage() {  return stage; }
    // Update is called once per frame
    void Update()
    {
        if (GlobalEventSystem.isPaused()) { return; }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (!frozen) rb.velocity = new Vector2(horizontal * 7.0f, vertical * 7.0f);
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(facing[0], 0), 4.5f, LayerMask.GetMask("NPC"));


            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("Tour Guide"))
                {
                    if (stage == 1) { dialogue.triggerDialogue("Otemon Gate"); }
                    else if (stage == 2) dialogue.triggerDialogue("Stone Walls");
                    else if (stage == 3) dialogue.triggerDialogue("Nishinomaru Bailey");
                } else if (hit.collider.name.Equals("Makoto"))
                {
                    hit.collider.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    dialogue.triggerDialogue("Makoto");
                }

            }
        }

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
        } else
        {
            aoiAnim.SetInteger("VMoving", 0);
            aoiAnim.SetInteger("HMoving", 0);
        }
        
    }

    public void minigame() // go to minigame
    {
        if (stage == 1)
        {
            minigamePlayer.SetActive(true);
            minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);

            frozen = true;
            minigame1objs[0].SetActive(true);
            minigame1objs[1].SetActive(true);
            minigame1objs[2].SetActive(true);
            minigame1objs[3].SetActive(true);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame1objs[2].GetComponent<PolygonCollider2D>();
            transform.position = new Vector2(105, 41);
            aoi.transform.position = new Vector2(98, 41);
        }
        else if (stage == 2)
        {
            minigamePlayer.SetActive(true);
            minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);
            frozen = true;
            minigame2objs[0].SetActive(true);
            minigame2objs[1].SetActive(true);
            minigame2objs[3].SetActive(true);
            minigame2objs[4].SetActive(true);
            minigame2objs[5].SetActive(true);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame2objs[2].GetComponent<PolygonCollider2D>();
            transform.position = new Vector2(382, 25);
            aoi.transform.position = new Vector2(387, 25);
        } else if (stage == 3)
        {
            minigamePlayer.SetActive(true);
            minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);
            frozen = true;
            cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame3objs[0].GetComponent<PolygonCollider2D>();
            transform.position = new Vector2(600, 38);
            aoi.transform.position = new Vector2(605, 38);
            minigame3objs[1].SetActive(true);
            minigame3objs[2].SetActive(true);
            minigame3objs[3].SetActive(true);
            minigame3objs[4].SetActive(true);
            minigame3objs[5].SetActive(true);
        }
    }

    public void nextStage() // next overworld area
    {

        if (stage == 2) // 2nd overworld area
        {

            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            minigame1objs[0].SetActive(false);
            minigame1objs[1].SetActive(false);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            minigamePlayer.GetComponent<PlayerMove>().setPos(new Vector2(193, 34));
            minigamePlayer.GetComponent<PlayerMove>().setOgPos(new Vector2(193, 34));
            minigamePlayer.SetActive(false);
            frozen = false;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage2objs[0].GetComponent<PolygonCollider2D>();
        } else if (stage == 3) // 3rd overworld area
        {
            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            minigame2objs[0].SetActive(false);
            minigame2objs[1].SetActive(false);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            minigamePlayer.GetComponent<PlayerMove>().setPos(new Vector2(444, 34));
            minigamePlayer.GetComponent<PlayerMove>().setOgPos(new Vector2(444, 34));
            minigamePlayer.SetActive(false);
            frozen = false;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage3objs[0].GetComponent<PolygonCollider2D>();
        }
        else if (stage == 4) // 4th overworld area
        {
            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage4objs[0].GetComponent<PolygonCollider2D>();
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            minigamePlayer.SetActive(false);
            minigame3objs[1].SetActive(false);
            minigame3objs[2].SetActive(false);
            minigame3objs[3].SetActive(false);
            minigame3objs[4].SetActive(false);
            minigame3objs[5].SetActive(false);
            frozen = false;
        }
        
    }

    public void lostMinigame() // go back to last overworld area
    {
        if(stage == 1)
        {
            frozen = false;
            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            transform.position = new Vector2(-40, 31);
            aoi.transform.position = new Vector2(-34, 31);
            minigamePlayer.SetActive(false);
            minigame1objs[0].SetActive(false);
            minigame1objs[1].SetActive(false);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage1objs[0].GetComponent<PolygonCollider2D>();
        } else if (stage == 2)
        {
            frozen = false;
            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            minigamePlayer.SetActive(false);
            transform.position = new Vector2(105, 41);
            aoi.transform.position = new Vector2(98, 41);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage2objs[0].GetComponent<PolygonCollider2D>();
        } else if (stage == 3)
        {
            frozen = false;
            minigamePlayer.GetComponent<PlayerMove>().healthText.text = "";
            minigamePlayer.SetActive(false);
            minigame3objs[1].SetActive(false);
            minigame3objs[2].SetActive(false);
            minigame3objs[3].SetActive(false);
            minigame3objs[4].SetActive(false);
            minigame3objs[5].SetActive(false);
            transform.position = new Vector2(382, 25);
            aoi.transform.position = new Vector2(387, 25);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage3objs[0].GetComponent<PolygonCollider2D>();
        }
    }

    public void setStage(int stage) { this.stage = stage; }
}

