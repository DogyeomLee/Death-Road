using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("AudioSource (Optional)")]
    [Tooltip("비어 있으면 Awake에서 자동 생성한다. BGM 전용 AudioSource.")]
    [SerializeField] public AudioSource bgmSource;
    [Tooltip("비어 있으면 Awake에서 자동 생성한다. SFX 전용 AudioSource.")]
    [SerializeField] public AudioSource sfxSource;

    [SerializeField ] public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        EnsureAudioSources();
    }

    public void PlayBgm(AudioClip bgmClip, float volume = 1f, bool loop = true)
    {
        if (bgmClip == null)
        {
            Debug.LogWarning("[SoundManager] PlayBgm 실패: bgmClip이 null입니다.");
            return;
        }

        if (bgmSource == null)
        {
            Debug.LogWarning("[SoundManager] PlayBgm 실패: bgmSource가 없습니다.");
            return;
        }

        bgmSource.clip = bgmClip;
        bgmSource.loop = loop;
        bgmSource.volume = Mathf.Clamp01(volume);
        bgmSource.Play();
    }
    public void StopBgm()
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("[SoundManager] StopBgm 실패: bgmSource가 없습니다.");
            return;
        }

        bgmSource.Stop();
    }

    public void PlaySfxOneShot(AudioClip sfxClip, float volumeScale = 1f)
    {
        if (sfxClip == null)
        {
            Debug.LogWarning("[SoundManager] PlaySfxOneShot 실패: sfxClip이 null입니다.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("[SoundManager] PlaySfxOneShot 실패: sfxSource가 없습니다.");
            return;
        }

        sfxSource.PlayOneShot(sfxClip, Mathf.Clamp01(volumeScale));
    }

    private void EnsureAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if(audioMixer ==  null)
        {
            audioMixer = GetComponent<AudioMixer>();
        }

        // 역할 분리를 위해 기본값을 명시적으로 지정한다.
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    //소리의 볼륨을 조절
    public void ChangeVolme(float vol)
    {
        if(vol <= 0)
        {
            return;
        }

        sfxSource.volume = vol;
    }
}

