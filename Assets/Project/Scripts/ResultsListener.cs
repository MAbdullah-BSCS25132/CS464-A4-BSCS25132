using System.Collections;
using UnityEngine;

public class ResultsListener : MonoBehaviour
{
    [SerializeField] private float delay = 1.2f;

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.GameWon += OnGameWon;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.GameWon -= OnGameWon;
        gm.GameLost -= OnGameLost;
    }

    private void OnGameWon()
    {
        StartCoroutine(ShowResults(true));
    }

    private void OnGameLost()
    {
        StartCoroutine(ShowResults(false));
    }

    private IEnumerator ShowResults(bool won)
    {
        yield return new WaitForSecondsRealtime(delay);

        int coins = gm.CoinsCollected;
        int seconds = Mathf.FloorToInt(gm.Elapsed);

        RunResult.Won = won;
        RunResult.Seconds = seconds;

        if (GameState.IsEndless)
        {
            RunResult.Score = coins * 100 + seconds * 10;
            RunResult.Stars = seconds >= 60 ? 3 : (seconds >= 30 ? 2 : (seconds >= 10 ? 1 : 0));
            RunResult.Coins = coins * 8;
        }
        else if (GameState.IsTimeAttack)
        {
            RunResult.Score = coins * 100;
            RunResult.Stars = coins >= 15 ? 3 : (coins >= 10 ? 2 : (coins >= 5 ? 1 : 0));
            RunResult.Coins = coins * 8;
        }
        else
        {
            RunResult.Score = coins * 100 + gm.Health * 50;
            RunResult.Stars = won ? Mathf.Clamp(gm.Health, 1, 3) : 0;
            RunResult.Coins = coins * (won ? 10 : 5);
        }

        RunResult.BoostUsed = gm.BoostActive;
        if (RunResult.BoostUsed) RunResult.Coins *= 2;
        GameState.Coins += RunResult.Coins;

        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo("Results");
    }
}