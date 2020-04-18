using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountElapsedTime : MonoBehaviour
{
    private float timeElapsed = 0f;

    [SerializeField]
    public TextMeshProUGUI textDisplay;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        textDisplay.text = "Time: " + timeElapsed.ToString();
    }
}
