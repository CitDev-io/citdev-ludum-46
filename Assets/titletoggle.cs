using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titletoggle : MonoBehaviour
{
    [SerializeField] public GameObject toggleBox;

    void Start()
    {
        toggleBox.SetActive(false);        
    }


    public void ToggleTitleBox() {
        toggleBox.SetActive(!toggleBox.activeSelf);
    }
}
