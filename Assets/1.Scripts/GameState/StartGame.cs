using UnityEngine;

public class StartGame : MonoBehaviour
{
   public void OnClickGoButton()
    {
        GameSceneManager.Instance.LoadSceneAsyncByName("GameScene");
    }
}
