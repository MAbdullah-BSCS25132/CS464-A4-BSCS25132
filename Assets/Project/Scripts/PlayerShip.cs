using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] private RectTransform field;
    [SerializeField] private float baseSpeed = 1300f;
    [SerializeField] private float bottomOffset = 420f;
    [SerializeField] private float edgePadding = 80f;

    private RectTransform rt;
    private float speed;

    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    private void OnEnable()
    {
        string ship = GameState.SelectedShip;
        speed = baseSpeed * ShipStats.SpeedMultiplier(ship);

        Image img = GetComponent<Image>();
        if (img != null) img.color = ShipStats.ColorOf(ship);
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
            x = Mathf.MoveTowards(x, targetX, speed * Time.deltaTime);
        }

        rt.anchoredPosition = new Vector2(x, r.yMin + bottomOffset);
    }
}