using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;
    [SerializeField] private int sfxVoices = 4;
    [SerializeField] private int uiVoices = 2;
    [SerializeField] private float pitchSemitones = 2f;   // +-2 semitone

    private AudioSource[] sfx;
    private AudioSource[] ui;
    private int sfxNext;
    private int uiNext;

    private void Awake()
    {
        // Sirf ek SfxPlayer: doosra copy ho to khud hat jata hai
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfx = CreateVoices(sfxGroup, sfxVoices);
        ui = CreateVoices(uiGroup, uiVoices);
    }

    private AudioSource[] CreateVoices(AudioMixerGroup group, int count)
    {
        AudioSource[] pool = new AudioSource[count];

        for (int i = 0; i < count; i++)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = group;
            s.playOnAwake = false;
            s.loop = false;
            s.spatialBlend = 0f;
            s.ignoreListenerPause = true;
            pool[i] = s;
        }

        return pool;
    }

    // Gameplay ki awaazein (SFX group)
    public void Play(AudioClip clip, float basePitch = 1f, float volume = 1f)
    {
        Fire(sfx, ref sfxNext, clip, basePitch, volume);
    }

    // Menu ki awaazein (UI group), wohi player
    public void PlayUi(AudioClip clip, float volume = 1f)
    {
        Fire(ui, ref uiNext, clip, 1f, volume);
    }

    private void Fire(AudioSource[] pool, ref int next, AudioClip clip, float basePitch, float volume)
    {
        if (clip == null || pool == null || pool.Length == 0) return;

        AudioSource s = pool[next];
        next = (next + 1) % pool.Length;

        // Pitch = 2 ^ (semitones / 12)
        float semitones = Random.Range(-pitchSemitones, pitchSemitones);
        s.pitch = basePitch * Mathf.Pow(2f, semitones / 12f);
        s.PlayOneShot(clip, volume);
    }
}