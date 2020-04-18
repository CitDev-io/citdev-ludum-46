using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterSeconds : MonoBehaviour
{
    private float timeAlive = 0f;
    public float secondsTillDeath = 60f;

    void Start()
    {
        
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive >= secondsTillDeath) {
            Destroy(gameObject);
        }
    }
}
