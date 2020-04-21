using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteController : MonoBehaviour
{
    [SerializeField] public SpriteRenderer v1;
    [SerializeField] public SpriteRenderer v2;
    [SerializeField] public SpriteRenderer v3;
    [SerializeField] public SpriteRenderer v4;

    public float atATime = 25f;
    void Start() {
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse() {
        bool directionUp = false;
        while (true) {
            Color tmp = v1.color;
            float nowA = tmp.a * 255f;
            nowA = nowA + (atATime * (directionUp ? 1 : -1));

            if (nowA < 75) {
                nowA = 75;
                directionUp = true;
            }

            if (nowA > 255) {
                nowA = 255;
                directionUp = false;
            }
            tmp.a = nowA;
            Color next = new Color(1f, 1f, 1f, nowA/255f);
            v1.color = next;
            v2.color = next;
            v3.color = next;
            v4.color = next;
            yield return new WaitForSeconds(0.085f);
        }
        
    }

}
