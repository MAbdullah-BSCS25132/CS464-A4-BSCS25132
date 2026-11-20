using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ---- C# events (publisher) ----
    public event Action GameStarted;
    public event Action<int, int> Collected;   // coins jama, target
    public event Action<int> Damaged;          // bachi hui lives
    public event Action GameWon;
    public event Action GameLost;

    [SerializeField] private bool autoStart = true;
    [SerializeField] private int maxHealth = 3;

    public int Level { get; private set; } = 1;
    public int CoinsCollected { get; private set; }
    public int TargetCoins { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get { return maxHealth; } }
    public bool IsPlaying { get; private set; }

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
        if (autoStart && UnityEngine.SceneManagement.SceneManager.sceneCount == 1) StartGame();
    }

    public void StartGame()
    {
        Level = GameState.SelectedLevel;
        TargetCoins = 6 + Level * 4;
        CoinsCollected = 0;
        Health = maxHealth;
        IsPlaying = true;
        Time.timeScale = 1f;
        GameStarted?.Invoke();
    }

    public void AddCoin()
    {
        if (!IsPlaying) return;

        CoinsCollected++;
        Collected?.Invoke(CoinsCollected, TargetCoins);

        if (CoinsCollected >= TargetCoins) Finish(true);
    }

    public void TakeDamage()
    {
        if (!IsPlaying) return;

        Health--;
        Damaged?.Invoke(Health);

        if (Health <= 0) Finish(false);
    }

    private void Finish(bool won)
    {
        IsPlaying = false;

        if (won) GameWon?.Invoke();
        else GameLost?.Invoke();
    }
}