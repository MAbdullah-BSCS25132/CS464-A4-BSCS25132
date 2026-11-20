using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayFlow : MonoBehaviour
{
    [SerializeField] private CanvasGroup background;
    [SerializeField] private string gameplayScene = "Gameplay";

    private bool paused;

    private void OnEnable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown += OnScreenShown;
    }

    private void OnDisable()
    {
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.ScreenShown -= OnScreenShown;
    }

    private void OnScreenShown(ScreenId id)
    {
        // Pause: game ruk jata hai. Pause se Settings kholne par bhi ruka rehta hai.
        if (id == ScreenId.Pause) paused = true;
        else if (!(id == ScreenId.Settings && paused)) paused = false;

        Time.timeScale = paused ? 0f : 1f;

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
}