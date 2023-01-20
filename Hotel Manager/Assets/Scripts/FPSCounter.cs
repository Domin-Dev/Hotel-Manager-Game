using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private Text counter;
    void Start()
    {
        counter = GetComponent<Text>();
    }
    void Update()
    {
        counter.text = "FPS:" + ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
