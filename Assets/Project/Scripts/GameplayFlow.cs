using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameplayFlow : MonoBehaviour
{
    [SerializeField] private CanvasGroup background;
    [SerializeField] private string gameplayScene = "Gameplay";
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float normalCutoff = 22000f;
    [SerializeField] private float pausedCutoff = 700f;
    [SerializeField] private float cutoffTime = 0.3f;

    private const string CutoffParam = "MusicCutoff";

    private bool paused;
    private float cutoff;
    private Tween cutoffTween;

    private void OnEnable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown += OnScreenShown;
    }

    private void Start()
    {
        cutoff = normalCutoff;
        SetCutoff(cutoff);
    }

    private void OnDisable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown -= OnScreenShown;

        cutoffTween?.Kill();
        Time.timeScale = 1f;
    }

    private void OnScreenShown(ScreenId id)
    {
        // Pause: game ruk jata hai. Pause se Settings kholne par bhi ruka rehta hai.
        bool nowPaused;
        if (id == ScreenId.Pause) nowPaused = true;
        else if (id == ScreenId.Settings && paused) nowPaused = true;
        else nowPaused = false;

        Time.timeScale = nowPaused ? 0f : 1f;

        if (nowPaused != paused)
        {
            paused = nowPaused;
            FadeMusicCutoff(paused ? pausedCutoff : normalCutoff);
        }

        // Gameplay mein menu ka background chhupa do, taake gameplay nazar aaye
        if (background != null)
        {
            background.DOKill();
            background.DOFade(id == ScreenId.Gameplay ? 0f : 1f, 0.25f).SetUpdate(true);
        }

        // Main Menu par aane par gameplay scene free kar do
        if (id == ScreenId.MainMenu && SceneManager.GetSceneByName(gameplayScene).isLoaded)
            SceneManager.UnloadSceneAsync(gameplayScene);
    }

    // Music ka low-pass cutoff dheere dheere badalta hai (unscaled time, pause mein bhi chalta hai)
    private void FadeMusicCutoff(float target)
    {
        cutoffTween?.Kill();
        cutoffTween = DOTween.To(() => cutoff, x =>
        {
            cutoff = x;
            SetCutoff(x);
        }, target, cutoffTime).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    private void SetCutoff(float value)
    {
        if (mixer == null) return;

        if (!mixer.SetFloat(CutoffParam, value))
            Debug.LogWarning("GameplayFlow: mixer mein '" + CutoffParam + "' naam ka exposed parameter nahi mila.");
    }
}