using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void NoParamDelegate();
public delegate void BoolDelegate(bool Bool);
public delegate void GameObjectDelegate(GameObject gameObject);
public delegate void Vector3Delegate(Vector3 vector3);

public class EventManager : Singleton<EventManager>
{
    public NoParamDelegate OnPlayerJumpSuccessful;
    public NoParamDelegate OnPlayerJumpFailed;
    public Vector3Delegate OnPlayerDropPlantSuccess;
    public NoParamDelegate OnPlayerDropPlantFailure;
    public NoParamDelegate OnPlayerPickupPlantSuccess;
    public NoParamDelegate OnPlayerPickupPlantFailure;
    public BoolDelegate OnPlayerChangeDirection;

    public GameObjectDelegate OnPlantHasEnteredScene;
    public NoParamDelegate OnPlantHasLeftScene;

    [SerializeField]
    public PlayerController currentCharacter;

    void Awake() {
        if (currentCharacter != null) {
            SubscribeToPlayerFeed(currentCharacter);
        }
        SubscribeToGameFeed();
    }

    void SubscribeToGameFeed() {
        GameSceneManager.Instance.OnPlantCreated += HandlePlantEnteredScene;
        GameSceneManager.Instance.OnPlantDestroyed += HandlePlantLeftScene;
    }

    void UnsubscribeToGameFeed() {
        GameSceneManager.Instance.OnPlantCreated -= HandlePlantEnteredScene;
        GameSceneManager.Instance.OnPlantDestroyed -= HandlePlantLeftScene;
    }

    public void SubscribeToPlayerFeed(PlayerController pc) {
        if (currentCharacter != null && currentCharacter != pc) {
            UnsubscribeFromPlayerFeed(currentCharacter);
        }
        pc.OnChangeDirection += HandlePlayerChangedDirection;
        pc.OnDropPlantFailure += HandlePlayerFailedToDropPlant;
        pc.OnDropPlantSuccess += HandlePlayerSuccessfullyDroppedPlant;
        pc.OnJumpFailed += HandlePlayerJumpFailed;
        pc.OnJumpSuccessful += HandlePlayerJumpSuccessful;
        pc.OnPickupPlantFailure += HandlePlayerFailedToPickUpPlant;
        pc.OnPickupPlantSuccess += HandlePlayerPickedUpPlant;
    }

    void UnsubscribeFromPlayerFeed(PlayerController pc) {
        pc.OnChangeDirection -= HandlePlayerChangedDirection;
        pc.OnDropPlantFailure -= HandlePlayerFailedToDropPlant;
        pc.OnDropPlantSuccess -= HandlePlayerSuccessfullyDroppedPlant;
        pc.OnJumpFailed -= HandlePlayerJumpFailed;
        pc.OnJumpSuccessful -= HandlePlayerJumpSuccessful;
        pc.OnPickupPlantFailure -= HandlePlayerFailedToPickUpPlant;
        pc.OnPickupPlantSuccess -= HandlePlayerPickedUpPlant;
    }

    private void HandlePlayerJumpSuccessful() {
        OnPlayerJumpSuccessful?.Invoke();
    }

    private void HandlePlayerJumpFailed() {
        OnPlayerJumpFailed?.Invoke();
    }

    private void HandlePlayerChangedDirection(bool direction) {
        OnPlayerChangeDirection?.Invoke(direction);
    }

    private void HandlePlayerFailedToDropPlant() {
        OnPlayerDropPlantFailure?.Invoke();
    }

    private void HandlePlayerSuccessfullyDroppedPlant(Vector3 v3) {
        OnPlayerDropPlantSuccess?.Invoke(v3);
    }

    private void HandlePlayerFailedToPickUpPlant() {
        OnPlayerPickupPlantFailure?.Invoke();
    }

    private void HandlePlayerPickedUpPlant() {
        OnPlayerPickupPlantSuccess?.Invoke();
    }

    private void HandlePlantEnteredScene(GameObject go) {
        OnPlantHasEnteredScene?.Invoke(go);
    }

    private void HandlePlantLeftScene() {
        OnPlantHasLeftScene?.Invoke();
    }
}