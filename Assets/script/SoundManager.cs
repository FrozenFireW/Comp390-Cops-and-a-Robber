using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource,Audio,PlayerAudio,Catch;
    [SerializeField]
    private AudioClip GemAudio, CoinAudio, HitAudio, CatchAudio,BoxAudio,SkillAudio,PotionAudio,TriggerAudio, SkillAudio2;
    public void Awake()
    {
        instance = this;
    }
    public void gemAudio()
    {
        audioSource.clip = GemAudio;
        audioSource.Play();
    }
    public void coinAudio()
    {
        audioSource.clip = CoinAudio;
        audioSource.Play();
    }
    public void hitAudio()
    {
        Audio.clip = HitAudio;
        Audio.Play();
    }
    public void catchAudio()
    {
        Catch.clip = CatchAudio;
        Catch.Play();
    }
    public void boxAudio()
    {
        audioSource.clip = BoxAudio;
        audioSource.Play();
    }
    public void skillAudio()
    {
        PlayerAudio.clip = SkillAudio;
        PlayerAudio.Play();
    }
    public void skillAudio2()
    {
        Audio.clip = SkillAudio2;
        Audio.Play();
    }
    public void potionAudio()
    {
        audioSource.clip = PotionAudio;
        audioSource.Play();
    }
    public void triggerAudio()
    {
        audioSource.clip = TriggerAudio;
        audioSource.Play();
    }
}
