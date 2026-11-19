using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeChannel channel = VolumeChannel.Master;
    [SerializeField] private TMP_Text valueText;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.onValueChanged.AddListener(OnChanged);
    }

    private void OnEnable()
    {
        float v = VolumeSettings.Get(channel);
        slider.SetValueWithoutNotify(v);
        Draw(v);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    private void OnChanged(float value)
    {
        VolumeSettings.Set(channel, value);
        Draw(value);
    }

    private void Draw(float value)
    {
        valueText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}