using UnityEngine;

public class GameAudioListener : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private float pitchVariation = 0.08f;

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.Collected += OnCollected;
        gm.Damaged += OnDamaged;
        gm.GameWon += OnGameWon;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.Collected -= OnCollected;
        gm.Damaged -= OnDamaged;
        gm.GameWon -= OnGameWon;
        gm.GameLost -= OnGameLost;
    }

    private void OnCollected(int coins, int target)
    {
        // Har coin par pitch thodi upar jati hai (combo jaisa ehsaas), saath mein random variation
        float rise = Mathf.Min(coins, 10) * 0.02f;
        Play(pickup, 1f + rise, true);
    }

    private void OnDamaged(int healthLeft)
    {
        Play(damage, 1f, true);
    }

    private void OnGameWon()
    {
        Play(win, 1f, false);
    }

    private void OnGameLost()
    {
        Play(lose, 1f, false);
    }

    private void Play(AudioClip clip, float basePitch, bool randomize)
    {
        if (clip == null || sfx == null) return;

        sfx.pitch = randomize ? basePitch + Random.Range(-pitchVariation, pitchVariation) : basePitch;
        sfx.PlayOneShot(clip);
    }
}