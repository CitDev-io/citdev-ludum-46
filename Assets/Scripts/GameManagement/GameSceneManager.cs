using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;


public class GameSceneManager : Singleton<GameSceneManager>
{
    public GameObjectDelegate OnPlantCreated;
    public NoParamDelegate OnPlantDestroyed;

    private GameObject plantInstance;
    [SerializeField]
    public GameObject plantPrefab;
    [SerializeField]
    public CinemachineVirtualCamera camera;
    private Transform previousTarget;

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
        previousTarget = camera.Follow;
        camera.Follow = plantInstance.transform;
    }

    private void HandlePlayerPickedUpPlant() {
        if (plantInstance == null) return;

        Destroy(plantInstance);
        OnPlantDestroyed?.Invoke();
        camera.Follow = previousTarget;
    }
}
