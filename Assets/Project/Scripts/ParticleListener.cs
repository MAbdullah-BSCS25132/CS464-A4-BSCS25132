using UnityEngine;

public class ParticleListener : MonoBehaviour
{
    [SerializeField] private RectTransform ship;
    [SerializeField] private Material material;
    [SerializeField] private Color pickupColor = new Color32(0xFF, 0xCA, 0x61, 255);
    [SerializeField] private Color hitColor = new Color32(0xC7, 0x28, 0x62, 255);

    private GameManager gm;
    private Camera cam;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        if (gm == null) return;

        gm.Collected += OnCollected;
        gm.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (gm == null) return;

        gm.Collected -= OnCollected;
        gm.Damaged -= OnDamaged;
    }

    private void OnCollected(int coins, int target)
    {
        Burst(pickupColor, 12, 3f, 0.22f, 0.5f);
    }

    private void OnDamaged(int current, int max)
    {
        Burst(hitColor, 22, 5f, 0.35f, 0.7f);
    }

    private void Burst(Color color, int count, float speed, float size, float life)
    {
        if (ship == null || material == null) return;

        if (cam == null) cam = Camera.main != null ? Camera.main : FindFirstObjectByType<Camera>();
        if (cam == null) return;

        // Ship Overlay canvas par hai, uski position screen pixels mein hoti hai
        Vector3 p = ship.position;
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, 10f));
        world.z = 0f;

        GameObject go = new GameObject("Burst");
        go.transform.position = world;

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = ps.main;
        main.duration = 0.5f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = life;
        main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.5f, speed);
        main.startSize = new ParticleSystem.MinMaxCurve(size * 0.5f, size);
        main.startColor = color;
        main.gravityModifier = 0f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;   // khatam hote hi khud hat jata hai

        // Ek hi burst, loop nahi
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)count) });

        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.05f;

        // Dheere dheere ghayab aur chhote
        ParticleSystem.ColorOverLifetimeModule fade = ps.colorOverLifetime;
        fade.enabled = true;
        Gradient g = new Gradient();
        g.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        fade.color = g;

        ParticleSystem.SizeOverLifetimeModule shrink = ps.sizeOverLifetime;
        shrink.enabled = true;
        shrink.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        ParticleSystemRenderer pr = go.GetComponent<ParticleSystemRenderer>();
        pr.sharedMaterial = material;
        pr.sortingOrder = 50;

        ps.Play();
    }
}