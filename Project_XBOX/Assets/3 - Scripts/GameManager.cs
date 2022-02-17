using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int targetFPS;

    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0;
    }


    public void LoadScene_Main(float seconds) => StartCoroutine(ILoadSceneMainWithDelay(seconds)); 

    IEnumerator ILoadSceneMainWithDelay(float i)
    {
        yield return new WaitForSeconds(i);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


}
