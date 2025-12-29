using UnityEngine;

public class musicManager : MonoBehaviour
{
    public static musicManager Instance;
    public AudioSource audioSource;

    [SerializeField] private AudioClip musicaActual;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            if (musicaActual != null)
            {
                audioSource.clip = musicaActual;
                audioSource.loop = true;
                audioSource.Play();
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            if (musicaActual != null && Instance.musicaActual != musicaActual)
            {
                Destroy(Instance.gameObject); 
                Instance = this;
                audioSource = GetComponent<AudioSource>();
                audioSource.clip = musicaActual;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
