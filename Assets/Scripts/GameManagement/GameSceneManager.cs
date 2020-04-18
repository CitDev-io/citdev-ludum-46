using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameSceneManager : Singleton<GameSceneManager>
{
    void Start() {
        SubscribeToPlayerEvents();
    }

    void OnDestroy() {
        UnsubscribeFromPlayerEvents();
    }
 
    private void SubscribeToPlayerEvents() {
        EventManager.Instance.OnPlayerPickupPlantSuccess += HandlePlayerPickedUpPlant;
        EventManager.Instance.OnPlayerDropPlantSuccess += HandlePlayerDroppedPlant;
    }

    private void UnsubscribeFromPlayerEvents() {
        EventManager.Instance.OnPlayerPickupPlantSuccess -= HandlePlayerPickedUpPlant;
        EventManager.Instance.OnPlayerDropPlantSuccess -= HandlePlayerDroppedPlant;
    }

    private void HandlePlayerDroppedPlant() {
        Debug.Log("I saw you drop the plant, i'm gonna make bad guys spawn");
    }

    private void HandlePlayerPickedUpPlant() {
        Debug.Log("I saw you pick up the plant, hiding bad guys");
    }
}
