using UnityEngine;

public static class WorldPos
{
    private static Camera cached;

    private static Camera Cam
    {
        get
        {
            if (cached == null)
                cached = Camera.main != null ? Camera.main : Object.FindFirstObjectByType<Camera>();
            return cached;
        }
    }

    // Overlay canvas ke UI element ki jagah screen pixels mein hoti hai, usay camera ke world mein badalte hain
    public static Vector3 FromUi(RectTransform rt)
    {
        Camera cam = Cam;
        if (cam == null) return Vector3.zero;

        Vector3 p = rt.position;
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, 10f));
        world.z = 0f;
        return world;
    }
}