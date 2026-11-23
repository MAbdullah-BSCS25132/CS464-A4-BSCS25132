using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelOption : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private Image background;
    [SerializeField] private Color unlockedColor = new Color32(0x1A, 0x21, 0x45, 255);
    [SerializeField] private Color lockedColor = new Color32(0x12, 0x17, 0x33, 255);

    private bool IsUnlocked
    {
        get { return SaveData.IsUnlocked(level); }
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Level Select har baar khulne par dobara check hota hai
    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        bool unlocked = IsUnlocked;
        numberText.text = level.ToString();

        if (!unlocked) labelText.text = "LOCKED";
        else if (SaveData.HasBest(level)) labelText.text = "BEST " + SaveData.FormatTime(SaveData.GetBest(level));
        else labelText.text = "NOT CLEARED";

        background.color = unlocked ? unlockedColor : lockedColor;
        numberText.alpha = unlocked ? 1f : 0.3f;
        labelText.alpha = unlocked ? 1f : 0.5f;
    }

    private void OnClick()
    {
        if (!IsUnlocked)
        {
            UIAudio.Play(UISound.Error);
            ((RectTransform)transform).DOShakeAnchorPos(0.35f, new Vector2(24f, 0f), 20, 90f, false, true).SetUpdate(true);
            return;
        }

        GameState.SelectedLevel = level;
        UIAudio.Play(UISound.Click);

        if (ScreenManager.Instance != null)
            ScreenManager.Instance.GoTo("Loadout");
    }
}