using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishPoint : MonoBehaviour
{
    public Text numFishCaught;
    public Text time;

    // Start is called before the first frame update
    public void SetFish(int points)
    {
        gameObject.SetActive(true);
        numFishCaught.text = "SCORE: " + points.ToString() + "X";

    }
    public void SetTime(int curTime)
    { 
        gameObject.SetActive(true);
        time.text = "TIME LEFT: " + curTime.ToString();

    }
}
