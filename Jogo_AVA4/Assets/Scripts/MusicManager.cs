using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public string sceneToDestroy;
    public string sceneToDestroy2;

    private AudioSource audioSource;

    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }

        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }
    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
        if(audioSource.clip != null)
        {
           if(resetSong)
            {
                audioSource.Stop();
            }
           audioSource.Play();

        }
    }
    public void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToDestroy)
        {
            Destroy(gameObject);
        }
        if (scene.name == sceneToDestroy2)
        {
            Destroy(gameObject);
        }
    }
}
