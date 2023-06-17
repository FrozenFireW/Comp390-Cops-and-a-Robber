using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer BGMAudioMixer;
    public AudioMixer AudioMixer;
    public GameObject SetMenu;
    public GameObject TTL;
    //public GameObject Player;
    public Text CoinNum;
    public Text GemNum;
    public void PauseGame()
    {
        SetMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        SetMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SetVolume(float volume)
    {
        BGMAudioMixer.SetFloat("BGM", volume);
    }
    public void SetVolume2(float volume)
    {
        AudioMixer.SetFloat("Audio", volume);
    }
    public void cleanData()
    {
        PlayerPrefs.SetInt("Coin", 0);
        CoinNum.text = 0.ToString();
        PlayerPrefs.SetInt("Gem", 0);
        GemNum.text = 0.ToString();
    }
    public void OpenTTL()
    {
        TTL.SetActive(true);
    }
    public void CloseTTL()
    {
        TTL.SetActive(false);
    }
}
