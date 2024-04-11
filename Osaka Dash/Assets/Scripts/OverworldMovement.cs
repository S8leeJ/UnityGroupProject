using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    float horizontal, vertical;
    Rigidbody2D rb;
    public CinemachineVirtualCamera cam;
    public GameObject minigamePlayer;
    public GameObject[] minigame1objs;
    public GameObject[] stage2objs;
    public GameObject[] minigame2objs;
    public GameObject[] stage3objs;
    public GameObject[] minigame3objs;
    public DialogueHimeji dialogue;
    bool frozen;
    int stage;
    [SerializeField] float[] facing = { 0f, 0f };
    // Start is called before the first frame update
    void Start()
    {
        // 105, 31
        frozen = false;
        stage = 1;
        rb = GetComponent<Rigidbody2D>();
        dialogue = GetComponent<DialogueHimeji>();
    }

    public int getStage() {  return stage; }
    // Update is called once per frame
    void Update()
    {
        if(GlobalEventSystem.isPaused()) { return; }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if(!frozen) rb.velocity = new Vector2(horizontal * 7.0f, vertical * 7.0f);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) facing[0] = -1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) facing[0] = 1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow)) facing[1] = 1;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow)) facing[1] = -1;
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(facing[0], facing[1]), 4.5f, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                dialogue.triggerDialogue("c");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Tour Guide"))
        {
            if (stage == 1) {
                /*minigamePlayer.SetActive(true);
                minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);
                frozen = true;
                minigame1objs[0].SetActive(true);
                minigame1objs[1].SetActive(true);
                cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
                cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame1objs[2].GetComponent<PolygonCollider2D>();
                transform.position = new Vector2(105, 31);
                stage++;*/
            } else if (stage == 2)
            {
                minigamePlayer.SetActive(true);
                minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);
                frozen = true;
                minigame2objs[0].SetActive(true);
                minigame2objs[1].SetActive(true);
                cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
                cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame2objs[2].GetComponent<PolygonCollider2D>();
                stage++;
                transform.position = new Vector2(382, 25);
            }
        }
    }

    public void mg1()
    {
        minigamePlayer.SetActive(true);
        minigamePlayer.GetComponent<PlayerMove>().setObjectsLeft(1);
        frozen = true;
        minigame1objs[0].SetActive(true);
        minigame1objs[1].SetActive(true);
        cam.GetComponent<CinemachineVirtualCamera>().Follow = minigamePlayer.transform;
        cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = minigame1objs[2].GetComponent<PolygonCollider2D>();
        transform.position = new Vector2(105, 31);
        stage++;
    }

    public void nextStage()
    {
        if (stage == 2)
        {
            minigame1objs[0].SetActive(false);
            minigame1objs[1].SetActive(false);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            minigamePlayer.GetComponent<PlayerMove>().setPos(new Vector2(193, 34));
            minigamePlayer.SetActive(false);
            frozen = false;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage2objs[0].GetComponent<PolygonCollider2D>();
        } else if (stage == 3)
        {
            minigame2objs[0].SetActive(false);
            minigame2objs[1].SetActive(false);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            // minigamePlayer.GetComponent<PlayerMove>().setPos(new Vector2(193, 34));
            minigamePlayer.SetActive(false);
            frozen = false;
            cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = stage3objs[0].GetComponent<PolygonCollider2D>();
        }
    }
}

