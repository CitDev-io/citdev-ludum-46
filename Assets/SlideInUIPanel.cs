using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInUIPanel : MonoBehaviour
{
    RectTransform rectTransform;
    Vector3 startingY;
    float speed = 1.5f;
    private float startTime;

    void Awake() {
        startingY = new Vector3(0f, 900f, 0f);
        GetComponent<RectTransform>().localPosition = startingY;
    }
    void Start()
    {
        startTime = Time.time;
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;

        rectTransform.localPosition = Vector3.Lerp(startingY, Vector3.zero, distCovered);
    }
}
