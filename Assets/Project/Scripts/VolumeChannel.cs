using UnityEngine;
using UnityEngine.Audio;

public enum VolumeChannel { Master, Music, SFX, UI }

public static class VolumeSettings
{
    private const float DefaultVolume = 0.8f;
    private static AudioMixer mixer;

    // Mixer ke exposed names: MasterVol, MusicVol, SFXVol, UIVol
    private static string Param(VolumeChannel c)
    {
        return c.ToString() + "Vol";
    }

    public static void Init(AudioMixer m)
    {
        mixer = m;

        foreach (VolumeChannel c in System.Enum.GetValues(typeof(VolumeChannel)))
            Apply(c, Get(c));
    }

    public static float Get(VolumeChannel c)
    {
        return PlayerPrefs.GetFloat("vol_" + c, DefaultVolume);
    }

    public static void Set(VolumeChannel c, float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("vol_" + c, value);
        Apply(c, value);
    }

    // Slider 0..1 ko decibel mein badalta hai (logarithmic).
    // 0 par seedha -80 dB (mute), taake log10(0) ka masla na ho.
    public static float ToDecibels(float value)
    {
        if (value <= 0.0001f) return -80f;
        return Mathf.Log10(value) * 20f;
    }

    private static void Apply(VolumeChannel c, float value)
    {
        if (mixer != null)
            mixer.SetFloat(Param(c), ToDecibels(value));
    }
}