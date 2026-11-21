using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private RectTransform field;
    [SerializeField] private RectTransform player;
    [SerializeField] private FallingItem coinPrefab;
    [SerializeField] private FallingItem asteroidPrefab;
    [SerializeField] private float playerRadius = 55f;

    [Header("Difficulty")]
    [SerializeField] private float baseSpeed = 610f;      // level 1 ki raftaar
    [SerializeField] private float speedPerStep = 220f;   // har agle level par kitni badhe

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

        // Difficulty 1 = basic. Level jitna upar, ya Endless mein waqt jitna guzre, utni zyada.
        float steps = gm.Difficulty - 1f;

        timer = Mathf.Max(0.22f, 0.82f - steps * 0.1f);
        float speed = baseSpeed + steps * speedPerStep;
        float coinChance = Mathf.Max(0.45f, 0.62f - steps * 0.03f);

        FallingItem prefab = Random.value < coinChance ? coinPrefab : asteroidPrefab;
        FallingItem item = Instantiate(prefab, field);

        Rect r = field.rect;
        RectTransform irt = (RectTransform)item.transform;
        irt.anchoredPosition = new Vector2(Random.Range(r.xMin + 80f, r.xMax - 80f), r.yMax + 80f);

        item.Init(field, player, speed, playerRadius);
    }
}