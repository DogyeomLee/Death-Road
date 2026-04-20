using UnityEngine;

public class ReplayButton : MonoBehaviour
{
   public void Replay()
    {
        GameSceneManager.Instance.ReloadCurrentScene();
    }
}
