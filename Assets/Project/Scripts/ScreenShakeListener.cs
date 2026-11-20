using DG.Tweening;
using UnityEngine;

public class ScreenShakeListener : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float damageStrength = 30f;
    [SerializeField] private float loseStrength = 55f;

    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.Damaged += OnDamaged;
        gm.GameLost += OnGameLost;
    }

    private void OnDisable()
    {
        if (target != null) target.DOKill(true);

        if (gm == null) return;

        gm.Damaged -= OnDamaged;
        gm.GameLost -= OnGameLost;
    }

    private void OnDamaged(int healthLeft)
    {
        Shake(damageStrength, 0.35f);
    }

    private void OnGameLost()
    {
        Shake(loseStrength, 0.6f);
    }

    private void Shake(float strength, float time)
    {
        target.DOKill(true);
        target.DOShakeAnchorPos(time, strength, 25, 90f, false, true).SetUpdate(true);
    }
}