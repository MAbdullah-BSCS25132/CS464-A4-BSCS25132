using UnityEngine;

public class MusicByScreen : MonoBehaviour
{
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
        if (AudioManager.Instance == null) return;

        switch (id)
        {
            case ScreenId.Gameplay:
            case ScreenId.Pause:
                AudioManager.Instance.PlayGameplayMusic();
                break;

            case ScreenId.Settings:
                // Settings Pause se bhi khulti hai, to music nahi badalni
                break;

            default:
                // Menu, Loading aur Results: shant menu music
                AudioManager.Instance.PlayMenuMusic();
                break;
        }
    }
}