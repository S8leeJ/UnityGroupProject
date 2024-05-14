using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetMove : MonoBehaviour
{
    public AudioSource soundEffect;
    public float moveSpeed = 5f;  
    public float rotationSpeed = 90f;  
    public float rotationDuration = 1f;  
    public float fishHoldDuration = 5f;
    public gameOver next;
    public FishPoint fishDisplay;
    private int fishCount;
    private Rigidbody2D rb;
    private bool isRotating = false;
    private bool isHoldingFish = false;
    private GameObject caughtFish = null;
    private Vector3 fishOriginalPosition;
    public int fishNeed;

    [SerializeField] Camera referenceCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fishCount = 0;
        StartCoroutine(LoadNextSceneAfterDelay("Sumiyoshi Taisha", 60f));

    }

    void Update()
    {
        if (!isHoldingFish)
        {
            fishDisplay.SetTime(0);
        }


        Vector3 newVelocity = Input.mousePosition;
        newVelocity -= referenceCam.WorldToScreenPoint(gameObject.transform.position);
        if (newVelocity.magnitude > moveSpeed) newVelocity = newVelocity.normalized * moveSpeed;
        rb.velocity = newVelocity;

     
        if (fishCount >= fishNeed)
        {
            Debug.Log("Fish all caught");
            //swap position of the net here to move on

            if (SceneManager.GetActiveScene().name == "GoldfishScoopL2")
            {
               SceneManager.LoadScene("Himeji Castle Planning");
            }
            else
            {
               SceneManager.LoadScene("GoldfishScoopL2");
            }
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isRotating) 
        {
            StartCoroutine(RotateAroundYAxis());
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
        {
            ReleaseFish();
        }

        if (isHoldingFish)
        {
            fishHoldDuration -= Time.deltaTime;
            fishDisplay.SetTime((int)(fishHoldDuration));

            if (fishHoldDuration <= 0f)
            {
                ReleaseFish();
            }
            else if (caughtFish != null)
            {
                caughtFish.transform.position = transform.position;
            }
        }
    }

    IEnumerator RotateAroundYAxis()
    {
        isRotating = true;
        soundEffect.Play();

        float originalRotation = transform.eulerAngles.z;
        float targetRotation = originalRotation + rotationSpeed;

        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            float currentRotation = Mathf.Lerp(originalRotation, targetRotation, elapsedTime / rotationDuration);
            transform.eulerAngles = new Vector3(0, currentRotation, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            float currentRotation = Mathf.Lerp(targetRotation, originalRotation, elapsedTime / rotationDuration);
            transform.eulerAngles = new Vector3(0, currentRotation, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isHoldingFish && (other.CompareTag("Fish") && isRotating))
        {
            caughtFish = other.gameObject;
            isHoldingFish = true;
            caughtFish.GetComponent<Collider2D>().enabled = false; 
            fishOriginalPosition = caughtFish.transform.position; 
        }
        else if (isHoldingFish && other.CompareTag("Bowl"))
        {
            DropFishInBowl(other.gameObject);
        }
        else if (other.gameObject.CompareTag("NPC"))
            {
            NPC npcScript = other.gameObject.GetComponent<NPC>();

             if (npcScript != null)
             {
              npcScript.DisplayDialog();
             }
            Debug.Log("here");
            }
        }

    void ReleaseFish()
    {
        isHoldingFish = false; 
        fishHoldDuration = 5f;
        if (caughtFish != null)
        {
            caughtFish.GetComponent<Collider2D>().enabled = true;
            caughtFish.transform.position = fishOriginalPosition; 
            caughtFish = null;
        }
    }

    void DropFishInBowl(GameObject bowl)
    {
        if (caughtFish != null)
        {
            isHoldingFish = false; 
            fishHoldDuration = 5f;
            if (caughtFish != null)
            {
                //add deduct points for b ad fish
                fishCount += 1;
                fishDisplay.SetFish(fishCount);
                caughtFish.GetComponent<Collider2D>().enabled = true; 
                Vector3 offsetPosition = new Vector3(-7, -4, 0);

                caughtFish.transform.position = offsetPosition;
                caughtFish = null;
            }
         }
    }
    IEnumerator LoadNextSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    //check to see if old manactually gets the 5 coins then set the coins to 0 (solves having 0 coins at the end problem
    // combine both scooping games into 1? or mak it more understandable
    //have a quest for the person
}
