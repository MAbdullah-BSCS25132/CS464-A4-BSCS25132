using UnityEngine;

public static class ShipStats
{
    // Player hamesha palette ka mid cyan, taake grayscale mein ek hi band mein rahe
    public static Color ColorOf(string ship)
    {
        return new Color32(0x22, 0xD3, 0xEE, 255);
    }

    public static float SpeedMultiplier(string ship)
    {
        return ship == "NOVA" ? 1.5f : 1f;
    }

    public static int ExtraLives(string ship)
    {
        return ship == "COMET" ? 1 : 0;
    }
}