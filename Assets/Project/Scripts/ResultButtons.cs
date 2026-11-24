using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsButtons : MonoBehaviour
{
    [SerializeField] private Button primaryButton;
    [SerializeField] private TMP_Text primaryLabel;
    [SerializeField] private Button secondaryButton;
    [SerializeField] private TMP_Text secondaryLabel;

    private bool canAdvance;
    private bool isFinal;

    private void Awake()
    {
        primaryButton.onClick.AddListener(OnPrimary);
        secondaryButton.onClick.AddListener(OnSecondary);
    }

    private void OnEnable()
    {
        bool wonClassic = RunResult.Won && GameState.IsClassic;

        isFinal = wonClassic && GameState.SelectedLevel >= GameState.MaxLevel;
        canAdvance = wonClassic && !isFinal;

        primaryLabel.text = canAdvance ? "NEXT LEVEL" : (isFinal ? "CREDITS" : "RESTART");

        if (canAdvance) secondaryLabel.text = "RESTART";
        else secondaryLabel.text = GameState.IsClassic ? "LEVEL SELECT" : "CHANGE MODE";
    }

    private void OnPrimary()
    {
        if (isFinal)
        {
            // Credits ke baad Back dabane par seedha Main Menu
            if (ScreenManager.Instance != null)
                ScreenManager.Instance.GoToWithBack("Credits", "MainMenu");
            return;
        }

        if (canAdvance) GameState.SelectedLevel++;
        Go("Loading");
    }

    private void OnSecondary()
    {
        if (canAdvance) Go("Loading");
        else Go(GameState.IsClassic ? "LevelSelect" : "ModeSelect");
    }

    private void Go(string screen)
    {
        GameplayFlow.Resume();

        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo(screen);
    }
}