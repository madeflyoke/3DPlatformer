using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    private int currentSceneIndex = 0;

    private void OnEnable()
    {
        EventManager.levelCompleteEvent += ChangeScene;
    }

    private void OnDisable()
    {
        EventManager.levelCompleteEvent -= ChangeScene;
    }

    private void ChangeScene()
    {
        currentSceneIndex++;
        //EventManager.CallOnChangeScene(currentSceneIndex);
        //SceneManager.LoadSceneAsync(currentSceneIndex);
    }
}
