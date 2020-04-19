using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showGunEnergy : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.OnPlayerGunChargeChange += HandleGunChargeChange;
    }

    void HandleGunChargeChange(int charge, int max) {       
        float percentCharge = (charge * 100f) / max;
        GetComponent<RectTransform>().sizeDelta = new Vector2(percentCharge, 100);
    }
}
