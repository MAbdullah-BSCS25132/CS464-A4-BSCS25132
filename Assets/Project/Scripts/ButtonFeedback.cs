using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
    IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private float pressedScale = 0.94f;
    [SerializeField] private float selectedScale = 1.06f;
    [SerializeField] private UISound pressSound = UISound.Click;
    [SerializeField] private bool playHover = true;

    private Tween tween;
    private Outline focusRing;
    private bool selected;

    private void Awake()
    {
        // Keyboard se chuni hui button par safed ring
        if (GetComponent<Graphic>() != null)
        {
            focusRing = gameObject.AddComponent<Outline>();
            focusRing.effectColor = new Color32(0xED, 0xFD, 0xFF, 255);
            focusRing.effectDistance = new Vector2(5f, -5f);
            focusRing.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playHover) return;

        // Hover sirf mouse par bajta hai. Phone par touch se double awaaz aati.
        ExtendedPointerEventData ext = eventData as ExtendedPointerEventData;
        if (ext != null && ext.pointerType == UIPointerType.MouseOrPen)
            UIAudio.Play(UISound.Hover);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ScaleTo(pressedScale, 0.08f, Ease.OutQuad);
        UIAudio.Play(pressSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ScaleTo(selected ? selectedScale : 1f, 0.30f, Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(selected ? selectedScale : 1f, 0.20f, Ease.OutQuad);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Mouse ya touch ke click par ring nahi, sirf keyboard se chunne par
        if (PointerIsDown()) return;

        selected = true;
        if (focusRing != null) focusRing.enabled = true;
        ScaleTo(selectedScale, 0.2f, Ease.OutBack);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        if (focusRing != null) focusRing.enabled = false;
        ScaleTo(1f, 0.15f, Ease.OutQuad);
    }

    // Enter dabane par bhi press ki awaaz aur halka punch
    public void OnSubmit(BaseEventData eventData)
    {
        UIAudio.Play(pressSound);
        tween?.Kill();
        tween = transform.DOPunchScale(Vector3.one * -0.06f, 0.2f, 6, 0.5f).SetUpdate(true);
    }

    private static bool PointerIsDown()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) return true;
        return false;
    }

    private void ScaleTo(float scale, float time, Ease ease)
    {
        tween?.Kill();
        tween = transform.DOScale(scale, time).SetEase(ease).SetUpdate(true);
    }

    private void OnDisable()
    {
        tween?.Kill();
        transform.localScale = Vector3.one;
        selected = false;
        if (focusRing != null) focusRing.enabled = false;
    }
}