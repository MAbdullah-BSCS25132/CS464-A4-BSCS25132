using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayFlow : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private CanvasGroup background;
    [SerializeField] private string gameplayScene = "Gameplay";
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float normalCutoff = 22000f;
    [SerializeField] private float pausedCutoff = 700f;
    [SerializeField] private float cutoffTime = 0.3f;

    private const string CutoffParam = "MusicCutoff";

    private float cutoff;
    private float cutoffTarget;
    private Tween cutoffTween;

    // Pause se wapas: time, static flag aur audio sab theek. Har load se pehle ye chalta hai.
    public static void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void OnEnable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown += OnScreenShown;
    }

    private void Start()
    {
        cutoff = normalCutoff;
        cutoffTarget = normalCutoff;
        SetCutoff(cutoff);
    }

    private void OnDisable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown -= OnScreenShown;

        cutoffTween?.Kill();
        Resume();
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return;

        ScreenManager sm = ScreenManager.Instance;
        if (sm == null || sm.IsBusy) return;

        switch (sm.Current)
        {
            case ScreenId.Gameplay:
                if (GameManager.Instance != null && GameManager.Instance.IsPlaying)
                {
                    UIAudio.Play(UISound.Pause);
                    sm.GoTo("Pause");
                }
                break;

            case ScreenId.Pause:
                UIAudio.Play(UISound.Resume);
                sm.Back();
                break;

            case ScreenId.ModeSelect:
            case ScreenId.LevelSelect:
            case ScreenId.Loadout:
            case ScreenId.Shop:
            case ScreenId.Settings:
            case ScreenId.Credits:
                UIAudio.Play(UISound.Back);
                sm.Back();
                break;
        }
    }

    private void OnScreenShown(ScreenId id)
    {
        // Pause: game ruk jata hai. Pause se Settings kholne par bhi ruka rehta hai.
        bool nowPaused = id == ScreenId.Pause || (id == ScreenId.Settings && IsPaused);

        IsPaused = nowPaused;
        Time.timeScale = nowPaused ? 0f : 1f;

        // Pause mein music dabi hui, wapas aane par saaf
        float want = nowPaused ? pausedCutoff : normalCutoff;
        if (!Mathf.Approximately(want, cutoffTarget))
        {
            cutoffTarget = want;
            FadeMusicCutoff(want);
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