using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timer;
    private static bool timeStarted = true;

    // Update is called once per frame
    private void Update()
    {
        if (timeStarted == true)
        {
            timer -= Time.deltaTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");

            timerText.text = (string.Format("{0}:{1}", minutes, seconds));
        }
    }
}