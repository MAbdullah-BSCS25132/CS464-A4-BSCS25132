using UnityEngine;

public class GameAudioListener : MonoBehaviour
{
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip shield;

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
        // Har coin par pitch thodi upar jati hai (combo jaisa ehsaas)
        float rise = Mathf.Min(coins, 10) * 0.02f;
        Play(pickup, 1f + rise);
    }

    private void OnDamaged(int current, int max)
    {
        Play(damage, 1f);
    }

    private void OnShieldBlocked()
    {
        // Shield ki alag awaaz na ho to damage ki awaaz oonchi pitch par
        if (shield != null) Play(shield, 1f);
        else Play(damage, 1.5f);
    }

    private void OnGameWon()
    {
        Play(win, 1f);
    }

    private void OnGameLost()
    {
        Play(lose, 1f);
    }

    // Sab kuch SfxPlayer se (wo object destroy nahi hota, to awaaz kat'ti nahi)
    private void Play(AudioClip clip, float basePitch)
    {
        if (SfxPlayer.Instance != null)
            SfxPlayer.Instance.Play(clip, basePitch);
    }
}