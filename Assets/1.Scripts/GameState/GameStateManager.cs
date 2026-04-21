using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Repair,
    Play,
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        currentState = GameState.None;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private GameState currentState;

    public GameState GetCurrentState => currentState;

    private void ChangeState(GameState nextState)
    {
        if (currentState == nextState)
        {
            return;
        }

        currentState = nextState;
        //이벤트 구현시 발동 구간// 여기
    }

    private void Update()
    {
        ChangeGameStateByScene();
    }

    public void ChangeGameStateByScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        switch (currentState)
        {
            case GameState.None:

                if (activeScene.name == "GameScene")
                {
                    ChangeState(GameState.Play);
                }
                break;
            case GameState.Repair:
                if (activeScene.name == "GameScene")
                {
                    ChangeState(GameState.Play);
                }
                else
                {
                    ChangeState(GameState.None);
                }
                break;
            case GameState.Play:
                if (activeScene.name == "RepairScene")
                {
                    ChangeState(GameState.Repair);
                }
                else if (activeScene.name != "GameScene")
                {
                    ChangeState(GameState.None);
                }
                break;
        }
    }

}
