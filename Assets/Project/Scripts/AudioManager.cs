using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private float fadeTime = 1.2f;
    [SerializeField, Range(0f, 1f)] private float musicLevel = 0.7f;

    private AudioSource active;
    private AudioSource idle;
    private AudioClip currentClip;

    private void Awake()
    {
        Instance = this;

        active = sourceA;
        idle = sourceB;

        foreach (AudioSource s in new[] { sourceA, sourceB })
        {
            s.playOnAwake = false;
            s.loop = true;
            s.ignoreListenerPause = true;
            s.volume = 0f;
        }
    }

    private void Start()
    {
        // Mixer ki values Start mein set hoti hain (Awake mein kabhi kabhi apply nahi hoti)
        VolumeSettings.Init(mixer);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    // Do AudioSource ke beech crossfade: purana dheere band, naya dheere shuru
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || clip == currentClip) return;
        currentClip = clip;

        AudioSource from = active;
        AudioSource to = idle;

        from.DOKill();
        to.DOKill();

        to.clip = clip;
        to.volume = 0f;
        to.Play();

        to.DOFade(musicLevel, fadeTime).SetUpdate(true);
        from.DOFade(0f, fadeTime).SetUpdate(true).OnComplete(() =>
        {
            if (from != active) from.Stop();
        });

        active = to;
        idle = from;
    }
}