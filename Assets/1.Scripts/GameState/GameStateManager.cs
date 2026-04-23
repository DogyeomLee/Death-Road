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

    private const string SCENE_GAME = "GameScene";
    private const string SCENE_REPAIR = "RepairScene";

    private GameState currentState;

    public GameState GetCurrentState => currentState;
    private void OnEnable()
    {
        // 씬이 로드될 때마다 이 함수가 실행되도록 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 메모리 누수 방지를 위해 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeGameStateByScene();
    }

    private void ChangeState(GameState nextState)
    {
        if (currentState == nextState)
        {
            return;
        }

        currentState = nextState;
        //이벤트 구현시 발동 구간// 여기
    }


    public void ChangeGameStateByScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        switch (activeScene.name)
        {
            case SCENE_GAME:
                UpgradeManager.Instance.ClearUndoStack();
                ChangeState(GameState.Play);
                break;
            case SCENE_REPAIR:
                ChangeState(GameState.Repair);
                break;
            default :
                UpgradeManager.Instance.ClearUndoStack();
                ChangeState(GameState.None);
                break;
        }
    }

}
