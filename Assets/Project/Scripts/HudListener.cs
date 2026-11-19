using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudListener : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private Image[] lives;
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
        gm.GameWon += OnGameWon;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.GameStarted -= OnGameStarted;
        gm.Collected -= OnCollected;
        gm.Damaged -= OnDamaged;
        gm.GameWon -= OnGameWon;
        gm.GameLost -= OnGameLost;
    }

    private void OnGameStarted()
    {
        bannerText.text = "";
        DrawScore(0, gm.TargetCoins);
        DrawLives(gm.Health);
    }

    private void OnCollected(int coins, int target)
    {
        DrawScore(coins, target);
        scoreText.transform.DOKill(true);
        scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6, 0.5f);
    }

    private void OnDamaged(int healthLeft)
    {
        DrawLives(healthLeft);

        if (healthLeft >= 0 && healthLeft < lives.Length)
        {
            lives[healthLeft].transform.DOKill(true);
            lives[healthLeft].transform.DOPunchScale(Vector3.one * 0.4f, 0.3f, 6, 0.5f);
        }
    }

    private void OnGameWon()
    {
        ShowBanner("LEVEL CLEAR");
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
        scoreText.text = "COINS " + coins + " / " + target;
    }

    private void DrawLives(int health)
    {
        for (int i = 0; i < lives.Length; i++)
            lives[i].color = i < health ? lifeOn : lifeOff;
    }
}