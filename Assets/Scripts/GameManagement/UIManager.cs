using UnityEngine;
using System.Collections;
using System;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    public GameObject Playing;
    public GameObject GameOver;
    public GameObject Paused;

    void Start()
    {
        Playing.SetActive(true);
        GameOver.SetActive(false);
        Paused.SetActive(false);
        EventManager.Instance.OnPlantDied += GameEnded;
        EventManager.Instance.OnGamePaused += Pause;
        EventManager.Instance.OnGameUnpaused += Unpause;
    }
    
    void Pause() {
        Playing.SetActive(false);
        GameOver.SetActive(false);
        Paused.SetActive(true);
    }

    void Unpause() {
        Playing.SetActive(true);
        GameOver.SetActive(false);
        Paused.SetActive(false);
    }

    void GameEnded() {
        Playing.SetActive(false);
        GameOver.SetActive(true);
        Paused.SetActive(false);
    }
}