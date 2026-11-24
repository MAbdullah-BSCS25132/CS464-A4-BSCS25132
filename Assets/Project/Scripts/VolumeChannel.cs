using UnityEngine;
using UnityEngine.Audio;

public enum VolumeChannel { Master, Music, SFX, UI }

public static class VolumeSettings
{
    public const float MinVolume = 0.0001f;
    private const float DefaultVolume = 0.8f;
    private static AudioMixer mixer;

    // Mixer ke exposed names
    private static string Param(VolumeChannel c)
    {
        switch (c)
        {
            case VolumeChannel.Music: return "MusicVolume";
            case VolumeChannel.SFX: return "SfxVolume";
            case VolumeChannel.UI: return "UIVol";
            default: return "MasterVol";
        }
    }

    public static void Init(AudioMixer m)
    {
        mixer = m;

        if (mixer == null)
        {
            Debug.LogError("VolumeSettings: AudioManager mein Mixer box khali hai.");
            return;
        }

        foreach (VolumeChannel c in System.Enum.GetValues(typeof(VolumeChannel)))
            Apply(c, Get(c));
    }

    public static float Get(VolumeChannel c)
    {
        // Music ki value SaveData mein hai (key "MusicVolume", default 1)
        if (c == VolumeChannel.Music) return SaveData.MusicVolume;
        return PlayerPrefs.GetFloat("vol_" + c, DefaultVolume);
    }

    public static void Set(VolumeChannel c, float value)
    {
        value = Mathf.Clamp(value, MinVolume, 1f);

        if (c == VolumeChannel.Music) SaveData.MusicVolume = value;
        else PlayerPrefs.SetFloat("vol_" + c, value);

        Apply(c, value);
    }

    // Slider 0.0001..1 ko decibel mein badalta hai: Log10(v) * 20
    // 1 -> 0 dB, 0.5 -> -6 dB, 0.0001 -> -80 dB (khamosh)
    public static float ToDecibels(float value)
    {
        if (value <= MinVolume) return -80f;
        return Mathf.Log10(value) * 20f;
    }

    private static void Apply(VolumeChannel c, float value)
    {
        if (mixer == null) return;

        if (!mixer.SetFloat(Param(c), ToDecibels(value)))
            Debug.LogWarning("VolumeSettings: mixer mein '" + Param(c) + "' naam ka exposed parameter nahi mila.");
    }
}