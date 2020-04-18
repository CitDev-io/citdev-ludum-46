using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void NoParamDelegate();
public delegate void BoolDelegate(bool Bool);

public class EventManager : Singleton<EventManager>
{
    public NoParamDelegate OnPlayerJumpSuccessful;
    public NoParamDelegate OnPlayerJumpFailed;
    public NoParamDelegate OnPlayerDropPlantSuccess;
    public NoParamDelegate OnPlayerDropPlantFailure;
    public NoParamDelegate OnPlayerPickupPlantSuccess;
    public NoParamDelegate OnPlayerPickupPlantFailure;
    public BoolDelegate OnPlayerChangeDirection;

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

    private void HandlePlayerSuccessfullyDroppedPlant() {
        OnPlayerDropPlantSuccess?.Invoke();
    }

    private void HandlePlayerFailedToPickUpPlant() {
        OnPlayerPickupPlantFailure?.Invoke();
    }

    private void HandlePlayerPickedUpPlant() {
        OnPlayerPickupPlantSuccess?.Invoke();
    }


    [SerializeField]
    public PlayerController currentCharacter;

    void Awake() {
        if (currentCharacter != null) {
            SubscribeToPlayerFeed(currentCharacter);
        }
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

    public void UnsubscribeFromPlayerFeed(PlayerController pc) {
        pc.OnChangeDirection -= HandlePlayerChangedDirection;
        pc.OnDropPlantFailure -= HandlePlayerFailedToDropPlant;
        pc.OnDropPlantSuccess -= HandlePlayerSuccessfullyDroppedPlant;
        pc.OnJumpFailed -= HandlePlayerJumpFailed;
        pc.OnJumpSuccessful -= HandlePlayerJumpSuccessful;
        pc.OnPickupPlantFailure -= HandlePlayerFailedToPickUpPlant;
        pc.OnPickupPlantSuccess -= HandlePlayerPickedUpPlant;
    }
}