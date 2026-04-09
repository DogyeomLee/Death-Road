using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static bool gameStart = false;

    public void GoButton()
    {
        SceneManager.LoadScene("GameScene");
        gameStart = true;   
    }
}
