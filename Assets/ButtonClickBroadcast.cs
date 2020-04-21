using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickBroadcast : MonoBehaviour
{
    public void ButtonWasClicked() {
        EventManager.Instance.ReportButtonClick();
    }
}
