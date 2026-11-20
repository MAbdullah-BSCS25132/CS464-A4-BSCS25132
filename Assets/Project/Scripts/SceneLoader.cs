using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "Gameplay";
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private float minShowTime = 1.5f;
    [SerializeField]
    private string[] tips =
    {
        "Drag left and right to steer",
        "Collect amber coins to win",
        "Red rocks cost one life"
    };

    private void OnEnable()
    {
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        // Pehle frame chhor do: Awake mein ScreenManager saare panels band karta hai
        yield return null;

        SetProgress(0f);
        if (tipText != null && tips.Length > 0)
            tipText.text = tips[Random.Range(0, tips.Length)];

        // Transition (slide in) khatam hone do
        while (ScreenManager.Instance != null && ScreenManager.Instance.IsBusy)
            yield return null;

        // Agar gameplay pehle se loaded hai (Retry) to unload karo taake naya shuru ho
        Scene existing = SceneManager.GetSceneByName(sceneName);
        if (existing.isLoaded)
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(existing);
            while (unload != null && !unload.isDone) yield return null;
        }

        // Asal async loading. allowSceneActivation false: 90% par ruk jata hai
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        float shown = 0f;
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.unscaledDeltaTime;

            float real = Mathf.Clamp01(op.progress / 0.9f);
            float timeCap = Mathf.Clamp01(elapsed / minShowTime);
            float target = Mathf.Min(real, timeCap);

            shown = Mathf.MoveTowards(shown, target, 1.5f * Time.unscaledDeltaTime);
            SetProgress(shown);

            if (real >= 1f && shown >= 1f) break;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.25f);

        // Ab scene activate karo, tab tak gameplay shuru nahi hoga
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        ScreenManager.Instance.GoTo("Gameplay");

        // Ye coroutine ScreenManager par chalti hai, kyunke Loading panel band ho jayega
        ScreenManager.Instance.StartCoroutine(StartGameWhenReady());
    }

    private IEnumerator StartGameWhenReady()
    {
        while (ScreenManager.Instance.IsBusy) yield return null;

        if (GameManager.Instance != null)
            GameManager.Instance.StartGame();
    }

    private void SetProgress(float p)
    {
        if (fill != null) fill.fillAmount = p;
        if (percentText != null) percentText.text = Mathf.RoundToInt(p * 100f) + "%";
    }
}