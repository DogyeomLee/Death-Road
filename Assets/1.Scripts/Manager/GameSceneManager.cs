using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (!IsValidSceneName(sceneName))
        {
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning($"[GameSceneManager] LoadSceneByName 실패: Build Settings에 없는 씬입니다. sceneName={sceneName}");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            Debug.LogWarning("[GameSceneManager] ReloadCurrentScene 실패: 현재 활성 씬이 유효하지 않습니다.");
            return;
        }

        LoadSceneByName(activeScene.name);
    }

    public AsyncOperation LoadSceneAsyncByName(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (!IsValidSceneName(sceneName))
        {
            return null;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning($"[GameSceneManager] LoadSceneAsyncByName 실패: Build Settings에 없는 씬입니다. sceneName={sceneName}");
            return null;
        }

        return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
    }

    private bool IsValidSceneName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("[GameSceneManager] sceneName이 비어 있습니다.");
            return false;
        }

        return true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


