using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPulse : MonoBehaviour
{
    [SerializeField] float pulseDuration = 1, maxAlpha = 0.8f, minAlpha = 0.5f;
    Image image;
    int maxalpha, minalpha;
    static float pulsetimer=0;

    // Start is called before the first frame update
    void Start()
    {
        pulseDuration /= (2 * Mathf.PI);
        image = GetComponent<Image>();
        maxalpha = (int)(maxAlpha * 255);
        minalpha = (int)(minAlpha * 255);
    }

    // Update is called once per frame
    void Update()
    {
        pulsetimer += Time.deltaTime;
        image.color = new Color(image.color.r, image.color.g, image.color.b, (maxalpha - minalpha) * Mathf.Sin(pulsetimer / pulseDuration) + minalpha);
    }
}
