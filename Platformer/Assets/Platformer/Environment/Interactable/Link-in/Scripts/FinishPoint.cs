using UnityEngine;
using Zenject;
using System.Collections;
public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)//player
        {
            StartCoroutine(LevelCompleteLogic());
        }
    }
    private IEnumerator LevelCompleteLogic()
    {
        yield return new WaitForSeconds(0.5f);
        EventManager.CallOnRequestGameState(GameState.LevelComplete);
    }
}
