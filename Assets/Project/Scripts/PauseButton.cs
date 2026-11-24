using UnityEngine;
using UnityEngine.UI;

public class PauseButtons : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(Restart);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    public void Restart()
    {
        // Resume() hamesha load se pehle, warna naya level frozen aata hai
        GameplayFlow.Resume();
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo("Loading");
    }

    public void QuitToMenu()
    {
        GameplayFlow.Resume();
        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo("MainMenu");
    }
}