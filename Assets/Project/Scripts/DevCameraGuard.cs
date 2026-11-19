using UnityEngine;

public class DevCameraGuard : MonoBehaviour
{
    private void Awake()
    {
        // Agar Main scene ka camera maujood hai to ye test camera hata do
        if (Camera.main != null)
            Destroy(gameObject);
    }
}