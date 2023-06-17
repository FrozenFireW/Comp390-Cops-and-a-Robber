using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPSetting : MonoBehaviour
{
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public GameObject empty1;
    public GameObject empty2;
    public GameObject empty3;
    public GameObject Dialog;
    public Collider2D coll;
    private Rigidbody2D rb;
    public AudioSource CatchAudio;
    int HP;
    int MaxHp;
    public static bool Buy;
    public float wait = 1f;
    public bool isHurt=false;
    public bool CanHurt = true;
    private Color color;
    private SpriteRenderer sr;
    public float flashTime;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        MaxHp = PlayerPrefs.GetInt("MaxHP");
        if(MaxHp <= 1)
        {
            MaxHp = 2;
        }
        HP = PlayerPrefs.GetInt("HP");
        rb = GetComponent<Rigidbody2D>();
        if (MaxHp >= 3)
        {
            Heart3.SetActive(true);
        }
        else
        {
            Heart3.SetActive(false);
            empty3.SetActive(false);
        }
        FlashHP();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHurt)
        {
            CanHurt = false;
            if (wait < 0)
            {
                isHurt = false;
                CanHurt = true;
            }
            else
            {
                wait -= Time.deltaTime;
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dog")
        {
            if (CanHurt)
            {
                Hurt();
                wait = 1f;
                isHurt = true;
            }
        }
        if (collision.tag == "Cop")
        {
            SoundManager.instance.catchAudio();
            Dialog.SetActive(true);
            Invoke(nameof(Restart), 2f);
        }
        if (collision.tag == "Health")
        {
            SoundManager.instance.potionAudio();
            if (MaxHp>=3) 
            {
                Heart3.SetActive(true);
                Heart2.SetActive(true);
                Heart1.SetActive(true);
            }
            else 
            {
                Heart2.SetActive(true);
                Heart1.SetActive(true);
            }
            HP = MaxHp;
            PlayerPrefs.SetInt("HP", HP);
            Destroy(collision.gameObject);
            
        }
        if(collision.tag == "Heart")
        {
            Heart3.SetActive(true);
            if (MaxHp < 3)
            {
                MaxHp++;
            }
            PlayerPrefs.SetInt("MaxHP", 3);
            HP = MaxHp;
            PlayerPrefs.SetInt("HP", HP);
            FlashHP();
            Destroy(collision.gameObject);
        }
    }
    void Hurt()
    {
        HP--;
        FlashColor(flashTime);
        PlayerPrefs.SetInt("HP",HP);
        FlashHP();
    }
    void Restart()
    {
        
        MaxHp = PlayerPrefs.GetInt("MaxHP");
        HP = MaxHp;
        PlayerPrefs.SetInt("HP",HP);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }
    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }
    void ResetColor()
    {
        sr.color = color;
    }
    void FlashHP()
    {
        if (HP < 3)
        {
            Heart3.SetActive(false);
            if (MaxHp >= 3)
            {
                empty3.SetActive(true);
            }

            {
                if (HP < 2)
                {
                    Heart2.SetActive(false);
                    empty2.SetActive(true);
                    if (HP < 1)

                    {
                        Heart1.SetActive(false);
                        empty1.SetActive(true);
                        SoundManager.instance.catchAudio();
                        Dialog.SetActive(true);
                        Invoke(nameof(Restart), 2f);
                    }
                }


            }
        }
        else
        {
            Heart3.SetActive(true);
            Heart2.SetActive(true);
            Heart1.SetActive(true);
            
        }
    }
}
