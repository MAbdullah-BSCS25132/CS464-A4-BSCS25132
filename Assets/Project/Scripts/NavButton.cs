using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavButton : MonoBehaviour
{
    public enum NavAction { GoTo, Back, None }

    [SerializeField] private NavAction action = NavAction.GoTo;
    [SerializeField] private ScreenId target = ScreenId.MainMenu;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (ScreenManager.Instance == null || action == NavAction.None) return;

        if (action == NavAction.Back)
        {
            UIAudio.Play(UISound.PanelClose);
            ScreenManager.Instance.Back();
        }
        else
        {
            UIAudio.Play(UISound.PanelOpen);
            ScreenManager.Instance.GoTo(target.ToString());
        }
    }
}