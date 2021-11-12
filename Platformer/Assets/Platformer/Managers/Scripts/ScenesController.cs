using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesController : MonoBehaviour
{
    private int currentSceneIndex=0;
    private void OnEnable()
    {
        //EventManager.levelCompleteEvent += SetNextScene;
        EventManager.startGameEvent += SetNextScene;
        EventManager.playerDieEvent += EndGame;
    }

    private void OnDisable()
    {
        //EventManager.levelCompleteEvent -= SetNextScene;
        EventManager.startGameEvent -= SetNextScene;
        EventManager.playerDieEvent -= EndGame;
    }
    private IEnumerator Loading(AsyncOperation operation)
    {
        while (operation.progress<1)
        {
            yield return null;
        }
        EventManager.CallOnSetScene(currentSceneIndex);
        yield return null;
    } 
    
    private void EndGame()
    {
        Time.timeScale = 0;
    }

    private void SetNextScene()
    {
        currentSceneIndex++;
        StartCoroutine(Loading(SceneManager.LoadSceneAsync(currentSceneIndex)));
    }

}
