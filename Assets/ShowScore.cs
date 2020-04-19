using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    TextMeshProUGUI textElement;
    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
        EventManager.Instance.OnPlayerScoreChanged += HandleScoreChange;
        HandleScoreChange(0);
    }

    void HandleScoreChange(long score) {
        textElement.text = minifyLong(score);
    }

    string minifyLong(long value)
    {
        if (value >= 100000000000)
            return (value / 1000000000).ToString("#,0") + " B";
        if (value >= 10000000000)
            return (value / 1000000000D).ToString("0.0") + " B";
        if (value >= 100000000)
            return (value / 1000000).ToString("#,0.0") + " M";
        if (value >= 10000000)
            return (value / 1000000D).ToString("0.0") + " M";
        if (value >= 100000)
            return (value / 1000).ToString("#,0.0") + " K";
        if (value >= 10000)
            return (value / 1000D).ToString("0.0") + " K";
        return value.ToString("#,0"); 
    }
}


