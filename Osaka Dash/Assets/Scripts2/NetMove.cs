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

    [SerializeField] Vector2 maxPosition, minPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fishCount = 1;
        StartCoroutine(LoadNextSceneAfterDelay("Sumiyoshi Taisha", 60f));

    }

    void Update()
    {
        if (!isHoldingFish)
        {
            fishDisplay.SetTime(0);
        }


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if (transform.position.y <= -2.5f && transform.position.x <= -4.5f)
        {
            if (transform.position.y <= -2.5f && movement.y < 0)
            {
                movement.y = 0;  
            }
            if (transform.position.x <= -4.5f && movement.x < 0)
            {
                movement.x = 0;  
            }
        }
 

        rb.velocity = movement * moveSpeed;
        if (fishCount >= 5)
        {
            Debug.Log("Fish all caught");


            if (SceneManager.GetActiveScene().name == "GoldfishScoopL2")
            {
                SceneManager.LoadScene("Sumiyoshi Taisha");
            }
            else
            {
                SceneManager.LoadScene("GoldfishScoopL2");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
        {
            StartCoroutine(RotateAroundYAxis());
        }

        if (Input.GetKeyDown(KeyCode.R))
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
        if (!isHoldingFish && (other.CompareTag("Fish") || other.CompareTag("BadFish")) && isRotating)
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

}
