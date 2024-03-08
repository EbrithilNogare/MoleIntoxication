using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioUIController : MonoBehaviour
{
    public GameObject AudioPanel;

    public AudioMixer Mixer;

    private AudioSlider[] slider;

    private void Start()
    {
        slider = GetComponentsInChildren<AudioSlider>();

        foreach (AudioSlider slider in slider)
        {
            if (Mixer.GetFloat(slider.Key, out float val))
            {
                slider.SetValue(Mathf.Pow(10, val / 20));
                slider.OnValueChanged += UpdateMixerValue;
            }
            else
            {
                slider.SetValue(0f);
                slider.Enable(false);
            }
        }

        AudioPanel.SetActive(false);
    }

    private void UpdateMixerValue(string key, float volume)
    {
        Mixer.SetFloat(key, Mathf.Log10(volume) * 20);
    }

    public void ToggleVisibility()
    {
        AudioPanel.SetActive(!AudioPanel.activeSelf);
    }
}
