using UnityEngine;

public class settingPanel : MonoBehaviour
{
    private void OnEnable()
    {
        // 패널이 켜질 때 (Pause)
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        // 패널이 꺼질 때 (Resume)
        Time.timeScale = 1f;
    }
}
