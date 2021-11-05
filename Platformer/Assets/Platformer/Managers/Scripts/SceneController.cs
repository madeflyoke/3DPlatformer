using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private int currentSceneIndex = 0;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //EventManager.CallOnSetScene(currentSceneIndex);
    }

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
        SceneManager.LoadSceneAsync(currentSceneIndex);
        
    }
}
