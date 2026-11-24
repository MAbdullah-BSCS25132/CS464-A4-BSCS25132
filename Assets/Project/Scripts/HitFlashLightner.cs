using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashListener : MonoBehaviour
{
    [SerializeField] private Image flash;
    [SerializeField] private float peakAlpha = 0.45f;
    [SerializeField] private float duration = 0.35f;

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.GameStarted += OnGameStarted;
        gm.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (flash != null) flash.DOKill();

        if (gm == null) return;

        gm.GameStarted -= OnGameStarted;
        gm.Damaged -= OnDamaged;
    }

    private void OnGameStarted()
    {
        SetAlpha(0f);
    }

    private void OnDamaged(int current, int max)
    {
        flash.DOKill();
        SetAlpha(peakAlpha);
        flash.DOFade(0f, duration).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    private void SetAlpha(float a)
    {
        Color c = flash.color;
        c.a = a;
        flash.color = c;
    }
}