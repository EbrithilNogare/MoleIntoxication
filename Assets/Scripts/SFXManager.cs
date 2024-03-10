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

    public AudioSource defaultAudioSource;
    public List<AudioSource> audioSources;

    private void Awake()
    {
        base.Awake();

        for (int i = 0; i < 7; i++)
        {
            audioSources.Add(Instantiate(defaultAudioSource,transform));
            audioSources[i].gameObject.SetActive(true);
        }

        audioSources[0].clip = Button;
        audioSources[1].clip = MoleEating;
        audioSources[2].clip = MushroomOn;
        audioSources[3].clip = MushroomOff;
        audioSources[4].clip = MushroomPurchase;
        audioSources[5].clip = Roots;
        audioSources[6].clip = ToxinPop;
    }

    public void Play(AudioClip clip)
    {
        defaultAudioSource.clip = clip;
        defaultAudioSource.Play();
    }

    public void PlayButton()
    {
        audioSources[0].Play();
        // Play(Button);
    }

    public void PlayMole()
    {
        audioSources[1].Play();
        // Play(MoleEating);
    }

    public void PlayMushroomOn()
    {
        audioSources[2].Play();
        // Play(MushroomOn);
    }

    public void PlayMushroomOff()
    {
        audioSources[3].Play();
        // Play(MushroomOff);
    }

    public void PlayMushroomPurchase()
    {
        audioSources[4].Play();
        // Play(MushroomPurchase);
    }

    public void PlayRoots()
    {
        audioSources[5].Play();
        // Play(Roots);
    }

    public void PlayToxinPop()
    {
        audioSources[6].Play();
        // Play(ToxinPop);
    }
}
