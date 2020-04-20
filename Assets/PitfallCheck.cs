using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallCheck : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("HIT");
        if (LayerMask.LayerToName(col.gameObject.layer) == "Plant") {
            GameSceneManager.Instance.ReportPlantInPit();
        }
        if (LayerMask.LayerToName(col.gameObject.layer) == "Player") {
            GameSceneManager.Instance.ReportPlayerInPit();
        }
    }
}
