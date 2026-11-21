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

    private void Awake()
    {
        primaryButton.onClick.AddListener(OnPrimary);
        secondaryButton.onClick.AddListener(OnSecondary);
    }

    private void OnEnable()
    {
        // Agla level sirf Classic mein, jeetne par, aur jab agla level maujood ho
        canAdvance = RunResult.Won && GameState.IsClassic && GameState.SelectedLevel < GameState.MaxLevel;

        primaryLabel.text = canAdvance ? "NEXT LEVEL" : "TRY AGAIN";

        if (canAdvance) secondaryLabel.text = "RETRY";
        else secondaryLabel.text = GameState.IsClassic ? "LEVEL SELECT" : "CHANGE MODE";
    }

    private void OnPrimary()
    {
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
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo(screen);
    }
}