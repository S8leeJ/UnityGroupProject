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
        {
            Time.timeScale = 0f;
            paused.SetActive(true);
            Debug.Log("Active");

        }
    }
    public void resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;

    }

    public void restart()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
