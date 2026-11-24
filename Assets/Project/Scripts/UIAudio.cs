using UnityEngine;

public enum UISound { None, Hover, Click, Back, PanelOpen, PanelClose, Pause, Resume, Success, Error }

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance { get; private set; }

    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip back;
    [SerializeField] private AudioClip panelOpen;
    [SerializeField] private AudioClip panelClose;
    [SerializeField] private AudioClip pause;
    [SerializeField] private AudioClip resume;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip error;

    private void Awake()
    {
        Instance = this;
    }

    public static void Play(UISound sound)
    {
        if (Instance == null || SfxPlayer.Instance == null) return;
        SfxPlayer.Instance.PlayUi(Instance.GetClip(sound));
    }

    private AudioClip GetClip(UISound sound)
    {
        switch (sound)
        {
            case UISound.Hover: return hover;
            case UISound.Click: return click;
            case UISound.Back: return back;
            case UISound.PanelOpen: return panelOpen;
            case UISound.PanelClose: return panelClose;
            case UISound.Pause: return pause;
            case UISound.Resume: return resume;
            case UISound.Success: return success;
            case UISound.Error: return error;
            default: return null;
        }
    }
}