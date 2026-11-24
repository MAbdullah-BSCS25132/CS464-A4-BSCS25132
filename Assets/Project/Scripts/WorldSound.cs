using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class WorldSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioMixerGroup output;
    [SerializeField] private AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField, Range(0f, 1f)] private float volume = 0.6f;

    private AudioSource source;
    private GameObject emitter;
    private RectTransform rt;
    private bool pausedByMe;

    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    // Start mein, taake Spawner ne jagah set kar li ho
    private void Start()
    {
        if (clip == null) return;

        emitter = new GameObject("WorldSound");
        SceneManager.MoveGameObjectToScene(emitter, gameObject.scene);
        emitter.transform.position = WorldPos.FromUi(rt);

        source = emitter.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = output;
        source.loop = true;
        source.playOnAwake = false;
        source.volume = volume;

        // 3D awaaz: Spatial Blend 1, rolloff aur distances
        source.spatialBlend = 1f;
        source.rolloffMode = rolloff;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.dopplerLevel = 0f;

        // Sab ki awaaz ek saath na ho, isliye random jagah se shuru
        source.time = Random.value * clip.length;
        source.Play();
    }

    private void LateUpdate()
    {
        if (emitter == null) return;

        emitter.transform.position = WorldPos.FromUi(rt);

        // Pause mein gameplay ki loops ruk jayein, Resume par wapas
        if (GameplayFlow.IsPaused && !pausedByMe)
        {
            source.Pause();
            pausedByMe = true;
        }
        else if (!GameplayFlow.IsPaused && pausedByMe)
        {
            source.UnPause();
            pausedByMe = false;
        }
    }

    private void OnDestroy()
    {
        if (emitter != null) Destroy(emitter);
    }
}