using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private RectTransform field;
    [SerializeField] private RectTransform player;
    [SerializeField] private FallingItem coinPrefab;
    [SerializeField] private FallingItem asteroidPrefab;
    [SerializeField] private float playerRadius = 55f;

    private GameManager gm;
    private float timer;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm != null) gm.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        if (gm != null) gm.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        timer = 0.6f;

        // purana saman saaf karo (Retry par kaam aata hai)
        foreach (Transform child in field)
        {
            if (child.GetComponent<FallingItem>() != null)
                Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (gm == null || !gm.IsPlaying) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        int level = gm.Level;
        timer = Mathf.Max(0.35f, 0.9f - level * 0.1f);
        float speed = 500f + level * 120f;

        FallingItem prefab = Random.value < 0.6f ? coinPrefab : asteroidPrefab;
        FallingItem item = Instantiate(prefab, field);

        Rect r = field.rect;
        RectTransform irt = (RectTransform)item.transform;
        irt.anchoredPosition = new Vector2(Random.Range(r.xMin + 80f, r.xMax - 80f), r.yMax + 80f);

        item.Init(field, player, speed, playerRadius);
    }
}