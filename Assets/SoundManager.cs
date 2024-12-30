using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public bool fxOn;
    public bool musicOn;

    private AudioSource audioSource;

    public AudioClip beenHitSound;
    public AudioClip bigHitSound;
    public AudioClip massiveHitSound;
    public AudioClip lilHitSound;
    public AudioClip hitSound;
    public AudioClip dashRenew;
    public AudioClip stepSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playDashRenewSound()
    {
        audioSource.PlayOneShot(dashRenew);
    }

    public void playBigHitSound()
    {
        audioSource.PlayOneShot(bigHitSound);
    }

    public void playLilHitSound()
    {
        audioSource.PlayOneShot(lilHitSound);
    }

    public void playMassiveHitSound()
    {
        audioSource.PlayOneShot(massiveHitSound);
    }

    public void playHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void playStepSound()
    {
        audioSource.volume = 0.25f;
        audioSource.PlayOneShot(stepSound);
        audioSource.volume = 0.5f;
    }
}