using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Naye screens hamesha list ke END mein add karo, warna Inspector mein purane ids badal jate hain.
public enum ScreenId
{
    Title,
    MainMenu,
    ModeSelect,
    LevelSelect,
    Loadout,
    Gameplay,
    Pause,
    Results,
    Shop,
    Settings,
    Credits,
    Loading
}

[DefaultExecutionOrder(-200)]
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    // Jab bhi koi naya screen dikhna shuru ho
    public event Action<ScreenId> ScreenShown;

    [Serializable]
    public class ScreenEntry
    {
        public ScreenId id;
        public CanvasGroup group;
    }

    [Header("Screens")]
    [SerializeField] private List<ScreenEntry> screens = new List<ScreenEntry>();
    [SerializeField] private ScreenId startScreen = ScreenId.Title;

    [Header("Transition")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float slideDistance = 120f;

    private readonly Dictionary<ScreenId, ScreenEntry> lookup = new Dictionary<ScreenId, ScreenEntry>();
    private readonly Stack<ScreenId> history = new Stack<ScreenId>();
    private ScreenId current;
    private bool hasCurrent;
    private bool busy;

    public bool IsBusy { get { return busy; } }
    public ScreenId Current { get { return current; } }

    private void Awake()
    {
        Instance = this;

        foreach (ScreenEntry entry in screens)
        {
            if (entry.group == null) continue;
            lookup[entry.id] = entry;
            entry.group.alpha = 0f;
            entry.group.interactable = false;
            entry.group.blocksRaycasts = false;
            entry.group.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Transition(startScreen, true);
    }

    // Button OnClick ke liye: screen ka naam string mein likho, e.g. "MainMenu"
    public void GoTo(string screenName)
    {
        if (!Enum.TryParse(screenName, out ScreenId target))
        {
            Debug.LogWarning("ScreenManager: unknown screen name " + screenName);
            return;
        }

        if (busy || !lookup.ContainsKey(target)) return;

        if (hasCurrent)
        {
            if (target == ScreenId.MainMenu || target == ScreenId.Title)
                history.Clear();
            else
                history.Push(current);
        }

        Transition(target, true);
    }

    // Ek screen kholta hai aur Back dabane par pehle se tay screen par bhejta hai
    public void GoToWithBack(string screenName, string backScreenName)
    {
        if (busy) return;

        ScreenId target;
        ScreenId backTo;
        if (!Enum.TryParse(screenName, out target) || !Enum.TryParse(backScreenName, out backTo)) return;
        if (!lookup.ContainsKey(target)) return;

        history.Clear();
        history.Push(backTo);
        Transition(target, true);
    }

    // Back button ke liye
    public void Back()
    {
        if (busy || history.Count == 0) return;
        Transition(history.Pop(), false);
    }

    private void Transition(ScreenId target, bool forward)
    {
        if (!lookup.ContainsKey(target)) return;

        busy = true;
        float dir = forward ? 1f : -1f;

        CanvasGroup next = lookup[target].group;
        RectTransform nextRt = (RectTransform)next.transform;

        CanvasGroup outgoing = null;
        if (hasCurrent && current != target) outgoing = lookup[current].group;

        next.gameObject.SetActive(true);
        next.alpha = 0f;
        next.interactable = false;
        next.blocksRaycasts = false;
        nextRt.anchoredPosition = new Vector2(slideDistance * dir, 0f);

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        if (outgoing != null)
        {
            RectTransform outRt = (RectTransform)outgoing.transform;
            outgoing.interactable = false;
            outgoing.blocksRaycasts = false;

            seq.Join(outgoing.DOFade(0f, duration * 0.8f).SetEase(Ease.InCubic));
            seq.Join(outRt.DOAnchorPosX(-slideDistance * dir, duration).SetEase(Ease.InCubic));
        }

        seq.Join(next.DOFade(1f, duration).SetEase(Ease.OutCubic));
        seq.Join(nextRt.DOAnchorPosX(0f, duration).SetEase(Ease.OutCubic));

        seq.OnComplete(() =>
        {
            if (outgoing != null) outgoing.gameObject.SetActive(false);

            next.interactable = true;
            next.blocksRaycasts = true;
            current = target;
            hasCurrent = true;
            busy = false;

            FocusFirst(next, target);
        });

        ScreenShown?.Invoke(target);
    }

    // Screen khulte hi pehla button select (keyboard ke liye)
    private void FocusFirst(CanvasGroup panel, ScreenId id)
    {
        if (EventSystem.current == null) return;

        // Gameplay aur Loading mein koi button select nahi hona chahiye
        if (id == ScreenId.Gameplay || id == ScreenId.Loading)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        Selectable first = panel.GetComponentInChildren<Selectable>();
        EventSystem.current.SetSelectedGameObject(first != null ? first.gameObject : null);
    }
}