using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDepartureTrigger : MonoBehaviour
{
    public float characterDisappearTimer = 3f;
    bool removeChar = false;
    GameObject playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!removeChar) return;
        if (GlobalEventSystem.isInDialogue) return;
        characterDisappearTimer -= Time.deltaTime;
        if (characterDisappearTimer < 0) playerCharacter.SetActive(false);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCharacter = other.gameObject;
            Destroy(playerCharacter.GetComponent<Collider2D>());
            Destroy(playerCharacter.GetComponent<PlayerMovement>());
            //Destroy(playerCharacter.GetComponent<Rigidbody2D>());
            DialogueSystem.instance.TriggerDialogue("Train Depart");
            removeChar = true;
        }
    }
}
