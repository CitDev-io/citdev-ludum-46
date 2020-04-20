using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraOnX : MonoBehaviour
{
    void LateUpdate() {
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, transform.position.z);
    }
}
