using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] private RectTransform field;
    [SerializeField] private float moveSpeed = 2000f;
    [SerializeField] private float bottomOffset = 420f;
    [SerializeField] private float edgePadding = 80f;

    private RectTransform rt;

    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    private void Update()
    {
        Rect r = field.rect;
        float x = rt.anchoredPosition.x;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.IsPlaying)
        {
            float targetX = x;

            Pointer pointer = Pointer.current;
            if (pointer != null && pointer.press.isPressed)
            {
                Vector2 local;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(field, pointer.position.ReadValue(), null, out local))
                    targetX = local.x;
            }

            Keyboard kb = Keyboard.current;
            if (kb != null)
            {
                float dir = 0f;
                if (kb.leftArrowKey.isPressed || kb.aKey.isPressed) dir -= 1f;
                if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) dir += 1f;
                if (dir != 0f) targetX = x + dir * 500f;
            }

            targetX = Mathf.Clamp(targetX, r.xMin + edgePadding, r.xMax - edgePadding);
            x = Mathf.MoveTowards(x, targetX, moveSpeed * Time.deltaTime);
        }

        rt.anchoredPosition = new Vector2(x, r.yMin + bottomOffset);
    }
}