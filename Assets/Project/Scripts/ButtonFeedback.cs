using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float pressedScale = 0.94f;
    [SerializeField] private UISound pressSound = UISound.Click;
    [SerializeField] private bool playHover = true;

    private Tween tween;

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
        ScaleTo(1f, 0.30f, Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(1f, 0.20f, Ease.OutQuad);
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
    }
}