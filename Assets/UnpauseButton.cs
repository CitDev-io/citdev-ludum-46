using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseButton : MonoBehaviour
{
    public void UnpauseTheGameDaddyo() {
        EventManager.Instance.RequestUnpauseFromMenu();
    }
}
