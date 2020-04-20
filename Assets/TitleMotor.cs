using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMotor : MonoBehaviour
{
    public bool runLeft = true;
    public float leftXBoundary = -28f;
    public float rightXBoundary = 28f;
    public float speed = 0.15f;
    void Start()
    {
        StartCoroutine(ChaseScene());
    }

    IEnumerator ChaseScene() {
        while (true) {
            transform.localScale = new Vector3((runLeft ? 1f : -1f), 1f, 1f);
            float nextXPosition = transform.position.x + (speed * (runLeft ? -1 : 1));
            transform.position = new Vector3(nextXPosition, transform.position.y, transform.position.z);
            if (nextXPosition > rightXBoundary || nextXPosition < leftXBoundary) {
                runLeft = !runLeft;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}

