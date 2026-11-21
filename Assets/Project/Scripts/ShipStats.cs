using UnityEngine;

public static class ShipStats
{
    public static Color ColorOf(string ship)
    {
        switch (ship)
        {
            case "NOVA": return new Color32(0xFF, 0x9F, 0x1C, 255);
            case "COMET": return new Color32(0x3D, 0xDC, 0x97, 255);
            default: return new Color32(0x22, 0xD3, 0xEE, 255);
        }
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