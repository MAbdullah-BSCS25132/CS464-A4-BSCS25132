using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudListener : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Image[] lives;   // purane life icons, ab zaroori nahi
    [SerializeField] private Color lifeOn = new Color32(0x22, 0xD3, 0xEE, 255);
    [SerializeField] private Color lifeOff = new Color32(0x2B, 0x34, 0x63, 255);

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.GameStarted += OnGameStarted;
        gm.Collected += OnCollected;
        gm.Damaged += OnDamaged;
        gm.ShieldBlocked += OnShieldBlocked;
        gm.Ticked += OnTicked;
        gm.GameWon += OnGameWon;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.GameStarted -= OnGameStarted;
        gm.Collected -= OnCollected;
        gm.Damaged -= OnDamaged;
        gm.ShieldBlocked -= OnShieldBlocked;
        gm.Ticked -= OnTicked;
        gm.GameWon -= OnGameWon;
        gm.GameLost -= OnGameLost;
    }

    private void OnGameStarted()
    {
        bannerText.text = "";

        if (modeText != null)
        {
            if (GameState.IsClassic) modeText.text = "CLASSIC - LEVEL " + gm.Level;
            else modeText.text = GameState.IsEndless ? "ENDLESS" : "TIME ATTACK";
        }

        if (shieldText != null) shieldText.gameObject.SetActive(gm.ShieldActive);

        DrawScore(0, gm.TargetCoins);

        if (healthBar != null) healthBar.Set(gm.Health, gm.MaxHealth);
        DrawLives(gm.Health);

        for (int i = 0; i < lives.Length; i++)
            lives[i].gameObject.SetActive(i < gm.MaxHealth);

        DrawIcons(gm.Health);
    }

    private void OnCollected(int coins, int target)
    {
        DrawScore(coins, target);
        scoreText.transform.DOKill(true);
        scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6, 0.5f);
    }

    private void OnDamaged(int current, int max)
    {
        if (healthBar != null) healthBar.Set(current, max);
        DrawLives(current);
        DrawIcons(current);

        if (livesText != null)
        {
            livesText.transform.DOKill(true);
            livesText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 6, 0.5f);
        }
    }

    private void OnShieldBlocked()
    {
        if (shieldText == null) return;

        shieldText.transform.DOKill(true);
        shieldText.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 6, 0.5f)
            .OnComplete(() => shieldText.gameObject.SetActive(false));
    }

    private void OnTicked(int seconds)
    {
        if (timeText == null) return;
        timeText.text = "TIME " + (seconds / 60) + ":" + (seconds % 60).ToString("00");
    }

    private void OnGameWon()
    {
        ShowBanner(GameState.IsTimeAttack ? "TIME UP" : "LEVEL CLEAR");
    }

    private void OnGameLost()
    {
        ShowBanner("GAME OVER");
    }

    private void ShowBanner(string text)
    {
        bannerText.text = text;
        bannerText.transform.DOKill(true);
        bannerText.transform.localScale = Vector3.one * 0.6f;
        bannerText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    private void DrawScore(int coins, int target)
    {
        scoreText.text = target > 0 ? "COINS " + coins + " / " + target : "COINS " + coins;
    }

    private void DrawLives(int health)
    {
        if (livesText != null) livesText.text = "LIVES " + Mathf.Max(0, health);
    }

    private void DrawIcons(int health)
    {
        for (int i = 0; i < lives.Length; i++)
            lives[i].color = i < health ? lifeOn : lifeOff;
    }
}