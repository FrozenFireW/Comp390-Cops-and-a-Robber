using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSetting : MonoBehaviour
{
    public GameObject LifeStore;
    public GameObject Heart;
    public GameObject Health;
    public GameObject HealthStore;
    public Text GemNum;
    public Text CoinNum;
    int Gem;
    int Coin;
    float wait;
    // Start is called before the first frame update
    
    public void Buy()
    {
        Gem = PlayerPrefs.GetInt("Gem");
        if (Gem >= 50)
        {
            Heart.SetActive(true);
            LifeStore.SetActive(false);
            Gem -= 50;
            PlayerPrefs.SetInt("Gem", Gem);
            GemNum.text = Gem.ToString();
        }
    }
    public void CoinBuy()
    {
        Coin = PlayerPrefs.GetInt("Coin");
        if (Coin >= 50)
        {
            Health.SetActive(true);
            HealthStore.SetActive(false);
            Coin -= 50;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();
        }
    }
    public void CloseDialog()
    {
        LifeStore.SetActive(false);
        HealthStore.SetActive(false);
    }

}
