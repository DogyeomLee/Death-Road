using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Audio;

public class setting : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;

 
    public void OpenSettings()
    {
        settingPanel.SetActive(true);
    }

    public void StartButton()
    {
        GameSceneManager.Instance.LoadSceneByName("GameScene");
    }

    public void MusicOnButton()
    {
        //-80f ¼Ò¸® ²ô±â
        SoundManager.Instance.audioMixer.SetFloat("BGM", 0f);
    }

    public void MusicOffButton()
    {
        SoundManager.Instance.audioMixer.SetFloat("BGM", -80f);
    }

    public void SoundOnButton()
    {
        SoundManager.Instance.audioMixer.SetFloat("SFX", 0f);
    }

    public void SoundOffButton()
    {
        SoundManager.Instance.audioMixer.SetFloat("SFX", -80f);
    }

    public void RestartButton()
    {
        GameSceneManager.Instance.ReloadCurrentScene();
    }

    public void GarageButton()
    {
        GameSceneManager.Instance.LoadSceneByName("RepairScene");
    }

    public void MenuButton()
    {
        GameSceneManager.Instance.LoadSceneByName("TitleScene");
    }

    public void ResumeButton()
    {
        settingPanel.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
