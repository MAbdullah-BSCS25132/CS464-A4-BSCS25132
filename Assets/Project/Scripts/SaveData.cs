using System.Globalization;
using UnityEngine;

public static class SaveData
{
    public const int LevelCount = 5;

    private const string UnlockedKey = "unlocked";
    private const string MusicVolumeKey = "MusicVolume";

    // Level ki key sirf yahan likhi jati hai
    private static string BestKey(int level)
    {
        return "best" + level;
    }

    // ---- Unlocked levels (default 1) ----
    public static int Unlocked
    {
        get { return Mathf.Clamp(PlayerPrefs.GetInt(UnlockedKey, 1), 1, LevelCount); }
    }

    public static bool IsUnlocked(int level)
    {
        return level <= Unlocked;
    }

    public static void Unlock(int level)
    {
        level = Mathf.Clamp(level, 1, LevelCount);
        if (level <= Unlocked) return;

        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    // ---- Best time per level ----
    public static bool HasBest(int level)
    {
        return PlayerPrefs.HasKey(BestKey(level));
    }

    public static float GetBest(int level)
    {
        return PlayerPrefs.GetFloat(BestKey(level), 0f);
    }

    // true wapas deta hai agar ye naya best time hai
    public static bool Record(int level, float seconds)
    {
        if (level < 1 || level > LevelCount) return false;

        if (HasBest(level) && GetBest(level) <= seconds) return false;

        PlayerPrefs.SetFloat(BestKey(level), seconds);
        PlayerPrefs.Save();
        return true;
    }

    // ---- Music volume (default 1), Chunk 5 mein kaam aayegi ----
    public static float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(MusicVolumeKey, 1f); }
        set { PlayerPrefs.SetFloat(MusicVolumeKey, Mathf.Clamp01(value)); }
    }

    public static string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        float rest = seconds - minutes * 60f;
        return minutes + ":" + rest.ToString("00.0", CultureInfo.InvariantCulture);
    }
}