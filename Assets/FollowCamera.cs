using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject player;
    public GameObject target;

    private float offset;

    void Start () 
    {
        target = player;
        offset = transform.position.x - player.transform.position.x;
        GameSceneManager.Instance.OnPlantCreated += ChangeTarget;
        GameSceneManager.Instance.OnPlantDestroyed += ChangeTargetToPlayer;
    }

    void OnDestroy() {
        GameSceneManager.Instance.OnPlantCreated -= ChangeTarget;
        GameSceneManager.Instance.OnPlantDestroyed -= ChangeTargetToPlayer;
    }

    void ChangeTarget(GameObject gameObject) {
        target = gameObject;
    }

    void ChangeTargetToPlayer() {
        target = player;
    }

    void LateUpdate () 
    {
        if (target == null) target = player;
        transform.position = new Vector3(target.transform.position.x + offset, transform.position.y, transform.position.z);
    }
}