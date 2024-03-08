using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private string _volumeKey;

    private Slider _slider;

    public float Volume => _slider.value;
    public string Key => _volumeKey;

    public System.Action<string, float> OnValueChanged;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.onValueChanged.AddListener(SliderMoved);
    }

    private void SliderMoved(float newVal)
    {
        OnValueChanged.Invoke(Key, newVal);
    }

    public void SetValue(float val)
    {
        _slider.SetValueWithoutNotify(val);
    }

    internal void Enable(bool enable)
    {
        _slider.interactable = enable;
    }
}
