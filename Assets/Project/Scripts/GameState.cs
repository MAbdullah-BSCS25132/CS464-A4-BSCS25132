using System;
using System.Collections.Generic;

public static class GameState
{
    public const int MaxLevel = SaveData.LevelCount;

    public static string SelectedMode = "CLASSIC";
    public static int SelectedLevel = 1;
    public static int Coins = 500;
    public static string SelectedShip = "FALCON";

    // Unlock ab PlayerPrefs mein save hota hai (SaveData ke zariye)
    public static int UnlockedLevel
    {
        get { return SaveData.Unlocked; }
        set { SaveData.Unlock(value); }
    }

    private static readonly HashSet<string> owned = new HashSet<string> { "FALCON" };
    private static readonly Dictionary<string, int> items = new Dictionary<string, int>();

    public static bool IsEndless
    {
        get { return string.Equals(SelectedMode, "ENDLESS", StringComparison.OrdinalIgnoreCase); }
    }

    public static bool IsTimeAttack
    {
        get { return string.Equals(SelectedMode, "TIME ATTACK", StringComparison.OrdinalIgnoreCase); }
    }

    public static bool IsClassic
    {
        get { return !IsEndless && !IsTimeAttack; }
    }

    public static bool IsOwned(string id)
    {
        return owned.Contains(id);
    }

    public static void Own(string id)
    {
        owned.Add(id);
    }

    public static bool TrySpend(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        return true;
    }

    public static void AddItem(string id)
    {
        int count;
        items.TryGetValue(id, out count);
        items[id] = count + 1;
    }

    public static int GetCount(string id)
    {
        int count;
        items.TryGetValue(id, out count);
        return count;
    }

    public static bool UseItem(string id)
    {
        if (GetCount(id) <= 0) return false;
        items[id]--;
        return true;
    }
}