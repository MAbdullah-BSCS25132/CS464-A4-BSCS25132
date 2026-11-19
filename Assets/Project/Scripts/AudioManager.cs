using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip click;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Mixer ki values Start mein set karni hoti hain (Awake mein kabhi kabhi apply nahi hoti).
        VolumeSettings.Init(mixer);

        if (music != null)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayClick()
    {
        if (click != null)
            sfxSource.PlayOneShot(click);
    }
}