using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ---- C# events (publisher) ----
    public event Action GameStarted;
    public event Action<int, int> Collected;   // coins jama, target (0 = koi target nahi)
    public event Action<int, int> Damaged;     // (current, max)
    public event Action ShieldBlocked;         // shield ne hit roka
    public event Action<int> Ticked;           // timer ke seconds (bache hue ya guzre hue)
    public event Action GameWon;
    public event Action GameLost;

    [SerializeField] private bool autoStart = true;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float timeAttackSeconds = 45f;

    public int Level { get; private set; } = 1;
    public int CoinsCollected { get; private set; }
    public int TargetCoins { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; } = 3;
    public bool IsPlaying { get; private set; }
    public bool ShieldActive { get; private set; }
    public bool BoostActive { get; private set; }
    public float Elapsed { get; private set; }
    public float Difficulty { get; private set; } = 1f;

    private int lastShown = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        // Akeli Gameplay scene chalane par khud shuru ho, Main ke saath load ho to Loading screen shuru karwati hai
        if (autoStart && UnityEngine.SceneManagement.SceneManager.sceneCount == 1) StartGame();
    }

    public void StartGame()
    {
        Level = GameState.IsClassic ? GameState.SelectedLevel : 1;
        TargetCoins = GameState.IsClassic ? 10 + (Level - 1) * 6 : 0;
        Difficulty = GameState.IsClassic ? Level : (GameState.IsTimeAttack ? 3f : 1f);

        // Ship ke hisaab se lives (Comet ko +1)
        MaxHealth = maxHealth + ShipStats.ExtraLives(GameState.SelectedShip);
        Health = MaxHealth;

        // Shop se kharide hue power-ups, har run mein ek ek istemal hota hai
        ShieldActive = GameState.UseItem("SHIELD");
        BoostActive = GameState.UseItem("BOOST");

        CoinsCollected = 0;
        Elapsed = 0f;
        lastShown = -1;
        IsPlaying = true;
        Time.timeScale = 1f;

        GameStarted?.Invoke();
    }

    private void Update()
    {
        if (!IsPlaying) return;

        Elapsed += Time.deltaTime;

        if (GameState.IsEndless)
            Difficulty = Mathf.Min(1f + Elapsed / 90f, 3f);

        int shown = GameState.IsTimeAttack
            ? Mathf.CeilToInt(Mathf.Max(0f, timeAttackSeconds - Elapsed))
            : Mathf.FloorToInt(Elapsed);

        if (shown != lastShown)
        {
            lastShown = shown;
            Ticked?.Invoke(shown);
        }

        if (GameState.IsTimeAttack && Elapsed >= timeAttackSeconds)
            Finish(true);
    }

    public void AddCoin()
    {
        if (!IsPlaying) return;

        CoinsCollected++;
        Collected?.Invoke(CoinsCollected, TargetCoins);

        if (TargetCoins > 0 && CoinsCollected >= TargetCoins) Finish(true);
    }

    public void TakeDamage()
    {
        if (!IsPlaying) return;

        // Shield pehla hit rok leta hai, life kam nahi hoti
        if (ShieldActive)
        {
            ShieldActive = false;
            ShieldBlocked?.Invoke();
            return;
        }

        Health--;
        Damaged?.Invoke(Health, MaxHealth);

        if (Health <= 0) Finish(false);
    }

    private void Finish(bool won)
    {
        IsPlaying = false;
        RunResult.NewBest = false;

        // Classic mein jeetne par best time save hota hai aur agla level unlock hota hai
        if (won && GameState.IsClassic)
        {
            RunResult.NewBest = SaveData.Record(Level, Elapsed);
            RunResult.BestTime = SaveData.GetBest(Level);
            SaveData.Unlock(Level + 1);
        }

        if (won) GameWon?.Invoke();
        else GameLost?.Invoke();
    }
}