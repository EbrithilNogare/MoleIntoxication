using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : SmartSingleton<SFXManager>
{
    public AudioClip Button;
    public AudioClip MoleEating;
    public AudioClip MushroomOn;
    public AudioClip MushroomOff;
    public AudioClip MushroomPurchase;
    public AudioClip Roots;

    public AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void PlayButton()
    {
        Play(Button);
    }

    public void PlayMole()
    {
        Play(MoleEating);
    }

    public void PlayMushroomOn()
    {
        Play(MushroomOn);
    }

    public void PlayMushroomOff()
    {
        Play(MushroomOff);
    }

    public void PlayMushroomPurchase()
    {
        Play(MushroomPurchase);
    }

    public void PlayRoots()
    {
        Play(Roots);
    }
}
