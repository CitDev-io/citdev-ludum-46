using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMobs : MonoBehaviour
{
    [SerializeField]
    public GameObject mobPrefab;
    public float minSec = 1.7f;
    public float maxSec = 6f;
    public float spawnMultiplierWhenPlantDown = 2f;
    bool plantInHand = false;

    void Start()
    {
        StartCoroutine(SpawnTime());
        EventManager.Instance.OnPlayerPickupPlantSuccess += HandlePlantPickup;
        EventManager.Instance.OnPlayerDropPlantSuccess += HandlePlantDrop;
    }

    void OnDestroy() {
        EventManager.Instance.OnPlayerPickupPlantSuccess -= HandlePlantPickup;
        EventManager.Instance.OnPlayerDropPlantSuccess -= HandlePlantDrop;
    }

    void HandlePlantPickup() {
        plantInHand = true;
    }

    void HandlePlantDrop(Vector3 v3) {
        plantInHand = false;
    }

    IEnumerator SpawnTime() {
        while (true) {
            float timeToNext = Random.Range(minSec, maxSec);
            if (!plantInHand) {
                timeToNext = timeToNext / spawnMultiplierWhenPlantDown;
            }
            yield return new WaitForSeconds(timeToNext);
            Instantiate(mobPrefab, transform.position, Quaternion.identity);
        }
    }
}
