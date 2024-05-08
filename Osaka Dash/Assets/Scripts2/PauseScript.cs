using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScript : MonoBehaviour
{
    public GameObject paused;
    // Start is called before the first frame update
    void Start()
    {
        paused.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void whenButtonClicked()
    {
        if (paused.activeInHierarchy == false)
            paused.SetActive(true);
    }
    public void resume()
    {
        gameObject.SetActive(false);
    }

    public void restart()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
