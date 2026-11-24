using UnityEngine;

public class GameAudioListener : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip shield;
    [SerializeField] private float pitchVariation = 0.08f;

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.Collected += OnCollected;
        gm.Damaged += OnDamaged;
        gm.ShieldBlocked += OnShieldBlocked;
        gm.GameWon += OnGameWon;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.Collected -= OnCollected;
        gm.Damaged -= OnDamaged;
        gm.ShieldBlocked -= OnShieldBlocked;
        gm.GameWon -= OnGameWon;
        gm.GameLost -= OnGameLost;
    }

    private void OnCollected(int coins, int target)
    {
        // Har coin par pitch thodi upar jati hai (combo jaisa ehsaas), saath mein random variation
        float rise = Mathf.Min(coins, 10) * 0.02f;
        Play(pickup, 1f + rise, true);
    }

    private void OnDamaged(int current, int max)
    {
        Play(damage, 1f, true);
    }

    private void OnShieldBlocked()
    {
        // Shield ki alag awaaz na ho to damage ki awaaz oonchi pitch par
        if (shield != null) Play(shield, 1f, true);
        else Play(damage, 1.5f, true);
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