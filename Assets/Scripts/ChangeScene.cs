using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SwapToScene(string sceneName) {
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
