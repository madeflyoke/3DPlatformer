using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesController : MonoBehaviour
{
    private int currentSceneIndex=0;
    private AsyncOperation loading;
    private void OnEnable()
    {
        EventManager.levelCompleteEvent += SetNextScene;
        EventManager.startGameEvent += SetNextScene;
    }

    private void OnDisable()
    {
        EventManager.levelCompleteEvent -= SetNextScene;
        EventManager.startGameEvent -= SetNextScene;
    }
    private IEnumerator Loading(AsyncOperation operation)
    {
        while (operation.progress<1)
        {
            Debug.Log("loading" + operation.progress);
            yield return null;
        }
        Debug.Log("finish");
        EventManager.CallOnSetScene(currentSceneIndex);
        yield return null;
    } 
    
    private void SetNextScene()
    {
        currentSceneIndex++;
        StartCoroutine(Loading(SceneManager.LoadSceneAsync(currentSceneIndex)));
    }

}
