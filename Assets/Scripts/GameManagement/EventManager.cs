using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void Vector2Delegate(Vector2 v2);
public delegate void DoubleIntDelegate(int int1, int int2);
public delegate void IntDelegate(int integer);
public delegate void NoParamDelegate();
public delegate void BoolDelegate(bool Bool);
public delegate void GameObjectDelegate(GameObject gameObject);
public delegate void Vector3Delegate(Vector3 vector3);

public class EventManager : Singleton<EventManager>
{
    public Vector2Delegate OnBadGuyDied;
    public NoParamDelegate OnBadGuyTookDamage;
    public NoParamDelegate OnBadGuyDealtDamage;
    public NoParamDelegate OnPlantDied;
    public DoubleIntDelegate OnPlantHealthChange;
    public DoubleIntDelegate OnPlayerGunChargeChange;
    public NoParamDelegate OnPlayerStoppedShooting;
    public NoParamDelegate OnPlayerStartedShooting;
    public NoParamDelegate OnPlayerShotFailedNoEnergy;
    public NoParamDelegate OnPlayerShotFailedNoGun;
    public NoParamDelegate OnPlayerLanded;
    public NoParamDelegate OnPlayerStartRunning;
    public NoParamDelegate OnPlayerStopRunning;
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
        GameSceneManager.Instance.OnPlantHealthChange += HandlePlantHealthChange;
        GameSceneManager.Instance.OnPlantDied += HandlePlantDied;
    }

    void UnsubscribeToGameFeed() {
        GameSceneManager.Instance.OnPlantCreated -= HandlePlantEnteredScene;
        GameSceneManager.Instance.OnPlantDestroyed -= HandlePlantLeftScene;
        GameSceneManager.Instance.OnPlantHealthChange -= HandlePlantHealthChange;
        GameSceneManager.Instance.OnPlantDied -= HandlePlantDied;
    }

    void HandlePlantHealthChange(int current, int max) {
        OnPlantHealthChange?.Invoke(current, max);
    }

    void HandlePlantDied() {
        OnPlantDied?.Invoke();
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
        pc.OnStartRunning += HandlePlayerStartRunning;
        pc.OnStopRunning += HandlePlayerStopRunning;
        pc.OnLanding += HandlePlayerLanding;
        pc.OnShootEnd += HandlePlayerStoppedShooting;
        pc.OnShootFailedNoEnergy += HandlePlayerFailedShotNoEnergy;
        pc.OnShootFailedNoGun += HandlePlayerFailedShotNoGun;
        pc.OnShootStart += HandlePlayerStartedShooting;
        pc.OnGunChargeChange += HandlePlayerGunChargeChange;
    }

    void UnsubscribeFromPlayerFeed(PlayerController pc) {
        pc.OnChangeDirection -= HandlePlayerChangedDirection;
        pc.OnDropPlantFailure -= HandlePlayerFailedToDropPlant;
        pc.OnDropPlantSuccess -= HandlePlayerSuccessfullyDroppedPlant;
        pc.OnJumpFailed -= HandlePlayerJumpFailed;
        pc.OnJumpSuccessful -= HandlePlayerJumpSuccessful;
        pc.OnPickupPlantFailure -= HandlePlayerFailedToPickUpPlant;
        pc.OnPickupPlantSuccess -= HandlePlayerPickedUpPlant;
        pc.OnStartRunning -= HandlePlayerStartRunning;
        pc.OnStopRunning -= HandlePlayerStopRunning;
        pc.OnLanding -= HandlePlayerLanding;
        pc.OnShootEnd -= HandlePlayerStoppedShooting;
        pc.OnShootFailedNoEnergy -= HandlePlayerFailedShotNoEnergy;
        pc.OnShootFailedNoGun -= HandlePlayerFailedShotNoGun;
        pc.OnShootStart -= HandlePlayerStartedShooting;
        pc.OnGunChargeChange -= HandlePlayerGunChargeChange;
    }

    private void HandlePlayerGunChargeChange(int charge, int max) {
        OnPlayerGunChargeChange?.Invoke(charge, max);
    }

    private void HandlePlayerStoppedShooting() {
        OnPlayerStartedShooting?.Invoke();
    }

    private void HandlePlayerFailedShotNoEnergy() {
        OnPlayerShotFailedNoEnergy?.Invoke();
    }

    private void HandlePlayerFailedShotNoGun() {
        OnPlayerShotFailedNoGun?.Invoke();
    }

    private void HandlePlayerStartedShooting() {
        OnPlayerStartedShooting?.Invoke();
    }

    private void HandlePlayerLanding() {
        OnPlayerLanded?.Invoke();
    }

    private void HandlePlayerStartRunning() {
        OnPlayerStartRunning?.Invoke();
    }

    private void HandlePlayerStopRunning() {
        OnPlayerStopRunning?.Invoke();
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

    public void ReportBadGuyDied(Vector2 location) {
        OnBadGuyDied?.Invoke(location);
    }

    public void ReportBadGuyTookDamage() {
        OnBadGuyTookDamage?.Invoke();
    }

    public void ReportBadGuyDealtDamage() {
        OnBadGuyDealtDamage?.Invoke();
    }
}