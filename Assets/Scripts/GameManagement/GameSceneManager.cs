using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameSceneManager : Singleton<GameSceneManager>
{
    public GameObjectDelegate OnPlantCreated;
    public NoParamDelegate OnPlantDestroyed;

    private GameObject plantInstance;
    [SerializeField]
    public GameObject plantPrefab;
    void Start() {
        SubscribeToEvents();
    }

    void OnDestroy() {
        UnsubscribeFromEvents();
    }
 
    private void SubscribeToEvents() {
        EventManager.Instance.OnPlayerPickupPlantSuccess += HandlePlayerPickedUpPlant;
        EventManager.Instance.OnPlayerDropPlantSuccess += HandlePlayerDroppedPlant;
    }

    private void UnsubscribeFromEvents() {
        EventManager.Instance.OnPlayerPickupPlantSuccess -= HandlePlayerPickedUpPlant;
        EventManager.Instance.OnPlayerDropPlantSuccess -= HandlePlayerDroppedPlant;
    }

    private void HandlePlayerDroppedPlant(Vector3 position) {
        var newPlant = Instantiate(plantPrefab, position, Quaternion.identity);
        plantInstance = newPlant;
        OnPlantCreated?.Invoke(newPlant);
        // TODO: Do we destroy the current instance/check for it to not dup?
    }

    private void HandlePlayerPickedUpPlant() {
        if (plantInstance == null) return;

        Destroy(plantInstance);
        OnPlantDestroyed?.Invoke();
    }
}
