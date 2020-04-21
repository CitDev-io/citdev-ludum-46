using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomToMoneyBar : MonoBehaviour
{
    float speed = 3f;
    private float startTime;
    public Transform target;
    public Vector3 startSpot;

    void Start()
    {
        startTime = Time.time;
        startSpot = transform.localPosition;
    }

    void Update()
    {
        if (target == null) return;
        float distCovered = (Time.time - startTime) * speed;
        transform.localPosition = Vector3.Lerp(startSpot, target.localPosition, distCovered);
    }

    public void SetTarget(Transform tt) {
        startTime = Time.time;
        startSpot = transform.localPosition;
        target = tt;
    }
}
