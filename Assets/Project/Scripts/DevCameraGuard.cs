using UnityEngine;

public class DevCameraGuard : MonoBehaviour
{
    private void Awake()
    {
        // Agar Main scene ka camera maujood hai to ye test camera (aur uska listener) hata do
        if (Camera.main != null)
        {
            AudioListener listener = GetComponent<AudioListener>();
            if (listener != null) listener.enabled = false;

            Destroy(gameObject);
        }
    }
}