using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public enum Kind { Coin, Asteroid }

    [SerializeField] private Kind kind = Kind.Coin;
    [SerializeField] private float radius = 40f;

    private RectTransform rt;
    private RectTransform field;
    private RectTransform player;
    private float speed;
    private float playerRadius;

    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    public void Init(RectTransform fieldRect, RectTransform playerRect, float fallSpeed, float playerR)
    {
        field = fieldRect;
        player = playerRect;
        speed = fallSpeed;
        playerRadius = playerR;
    }

    private void Update()
    {
        if (field == null || player == null) return;

        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.IsPlaying)
        {
            float dist = Vector2.Distance(rt.anchoredPosition, player.anchoredPosition);

            if (dist < radius + playerRadius)
            {
                if (kind == Kind.Coin) gm.AddCoin();
                else gm.TakeDamage();

                Destroy(gameObject);
                return;
            }
        }

        if (rt.anchoredPosition.y < field.rect.yMin - 150f)
            Destroy(gameObject);
    }
}