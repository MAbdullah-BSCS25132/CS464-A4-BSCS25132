using UnityEngine;

public static class FrameRateSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Setup()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}