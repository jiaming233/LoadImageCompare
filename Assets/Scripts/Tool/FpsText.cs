using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsText : MonoBehaviour
{
    private float updateInterval = 0.5f;
    private float timer;
    private FpsCounter fpsCounter;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        fpsCounter = new FpsCounter(updateInterval);
    }

    void Update()
    {
        fpsCounter.Update(Time.unscaledDeltaTime);

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0;
            Refresh();
        }
    }

    void Refresh()
    {
        text.text = string.Format("FPS {0:N2}", fpsCounter.CurrentFps);
    }
}