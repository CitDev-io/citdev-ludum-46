using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupCollisions : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll) {
        GameObject.Destroy(coll.gameObject);
    }
}
