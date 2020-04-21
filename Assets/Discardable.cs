using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discardable : MonoBehaviour
{
    public void DiscardMe() {
        Destroy(gameObject);
    }
}
