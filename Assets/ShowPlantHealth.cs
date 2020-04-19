using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPlantHealth : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.OnPlantHealthChange += HandlePlantHealthChange;
    }

    void HandlePlantHealthChange(int health, int max) {       
        float percentHealth = (health * 100f) / max;
        GetComponent<RectTransform>().sizeDelta = new Vector2(percentHealth, 100);
    }
}
