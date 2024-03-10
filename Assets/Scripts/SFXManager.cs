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
    public AudioClip ToxinPop;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        //audioSource.clip = clip;
        //audioSource.Play();
        AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position);
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

    public void PlayToxinPop()
    {
        Play(ToxinPop);
    }
}
