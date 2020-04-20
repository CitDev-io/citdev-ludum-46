using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMobs : MonoBehaviour
{
    [SerializeField]
    public GameObject mobPrefab;
    public float minSec = 1.7f;
    public float maxSec = 6f;

    void Start()
    {
        StartCoroutine(SpawnTime());
    }

    IEnumerator SpawnTime() {
        while (true) {
            Instantiate(mobPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSec, maxSec));
        }
    }
}
