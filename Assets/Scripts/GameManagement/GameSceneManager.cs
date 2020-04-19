using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;


public class GameSceneManager : Singleton<GameSceneManager>
{
    public DoubleIntDelegate OnPlantHealthChange;
    public NoParamDelegate OnPlantDied;
    public GameObjectDelegate OnPlantCreated;
    public NoParamDelegate OnPlantDestroyed;
    [SerializeField]
    public GameObject player;
    private GameObject plantInstance;
    [SerializeField]
    public GameObject plantPrefab;
    [SerializeField]
    public CinemachineVirtualCamera camera;
    public int plant_maxHealth = 1000;
    private int plant_health = 0;
    private int plant_toughness = 0;
    private int plant_decayAndHealRate = 12;
    private int plant_decayAmount = 6;
    private int plant_plantedGainAmount = 12;
    private bool plant_isAlive = true;
    private Coroutine plantHealthLoop;

    public Transform GetTarget() {
        if (plantInstance != null) {
            return plantInstance.transform;
        } else {
            return player.transform;
        }
    }

    void Awake() {
        plant_health = plant_maxHealth;
    }

    void Start() {
        SubscribeToEvents();
        plantHealthLoop = StartCoroutine(PlantHealthLoop());
    }

    void OnDestroy() {
        StopPlantHealthLoop();
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
        camera.Follow = plantInstance.transform;
        if (!plant_isAlive) {
            doPlantDeathPerformance();
        }
    }

    private void HandlePlayerPickedUpPlant() {
        if (plantInstance == null) return;

        Destroy(plantInstance);
        OnPlantDestroyed?.Invoke();
        camera.Follow = player.transform;
    }

    void StopPlantHealthLoop() {
        if (plantHealthLoop != null) {
            StopCoroutine(plantHealthLoop);
        }
    }

    IEnumerator PlantHealthLoop()
    {
        while(true) 
         { 
            bool isPlanted = plantInstance != null;
            int adjustment = -plant_decayAmount;
            if (isPlanted) {
                adjustment = plant_plantedGainAmount;
            }
            AdjustPlantHealth(adjustment);
            yield return new WaitForSeconds(1f/plant_decayAndHealRate);
         }
     }

     private void AdjustPlantHealth(int adjustment) {
         plant_health = Mathf.Clamp(plant_health + adjustment, 0, plant_maxHealth);
         OnPlantHealthChange?.Invoke(plant_health, plant_maxHealth);
         if (plant_health == 0) {
            StopPlantHealthLoop();
            OnPlantDied?.Invoke();
            if (plantInstance != null) {
                doPlantDeathPerformance();
            }
         }
     }

     void doPlantDeathPerformance() {
         if (plantInstance != null) {
             plantInstance.GetComponent<Animator>().SetBool("IsDead", true);
         }
     }
}
