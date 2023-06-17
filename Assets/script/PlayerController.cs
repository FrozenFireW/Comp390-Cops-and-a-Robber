using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public DogAI DogAI;
    public DogAI DogAI2;
    public DogAI DogAI3;
    public DogAI DogAI4;
    public DogAI DogAI5;
    public SpecialDogAi DogAI6;
    public SpecialCopAi CopsAI;
    public CopsAI CopsAI2;
    public CopsAI CopsAI3;
    public CopsAI CopsAI4;
   
    public Text CoinNum;
    public Text GemNum;
    public Image FrozenImage;
    public Image SpeedUpImage;
    public Image HideImage;
    public Image DashImage;
    public Image HideSkillImage;
    public Joystick joystick;

    public static bool isdoor;
    public static bool isshop;
    public Transform door;

    public Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Color color;

    private float speed = 2.5f;
    private float WaitTime =  0.7f;
    float WaitFrozenTime = 3f;
    float FrozenTime = 3f;
    float SpeedUpTime = 3f;
    float WaitSpeedUpTime = 3f;
    float WaitHideTime = 3f;
    float HideTime = 3f;
    float AnimHidetime= 1f;

    int Coin = 0;
    int Gem = 0;

    private float Dashtime = 0.4f;
    private float Dashspeed = 3f;
    private float DashtimeLeft;
    private float LastDash = -6f;
    private float DashCoolDown=6f;
    private bool isDash;

    private float Hidetime=3f;
    private float HidetimeLeft;
    private float LastHide = -18f;
    private float HideCoolDown = 18f;
    private bool isHide;
    public bool isKeyboard = false;

    private bool isHurt;
    private bool isFrozen;
    private bool isSpeedUp;
    private bool isUseDash;
    private bool isUseHide;
    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        Coin = PlayerPrefs.GetInt("Coin");
        CoinNum.text = Coin.ToString();
        Gem = PlayerPrefs.GetInt("Gem");
        GemNum.text = Gem.ToString();
        FrozenImage.fillAmount = 0;
        if (isdoor)
        {
            rb.position = door.position;
            isdoor = false;
            isshop = false;
            
        }
    }

    private void Update()
    {
        if (isKeyboard)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = (float)(Input.GetAxisRaw("Vertical") * 0.95);
        }
        else
        {
            movement.x = joystick.Horizontal;
            movement.y = (float) (joystick.Vertical * 0.95);
        }
       
        if (isFrozen)
        {
           
            if (WaitFrozenTime > 0)
            {
                FrozenImage.fillAmount -= 1.0f / FrozenTime * Time.deltaTime;
                WaitFrozenTime -= Time.deltaTime;
            }
            else
            {
                WaitFrozenTime = 3f;
                isFrozen = false;
                ReSetFrozen();
            }
            
        }

        if (isHide)
        {

            if (WaitHideTime > 0)
            {
                HideImage.fillAmount -= 1.0f / HideTime * Time.deltaTime;
                WaitHideTime -= Time.deltaTime;
            }
            else
            {
                WaitHideTime = 3f;
                ReSetHide();
            }

        }

        if (isSpeedUp)
        {
            if (WaitSpeedUpTime < 0)
            {
                speed = 2.5f;
                isSpeedUp = false;
                WaitSpeedUpTime = 3f;

            }
            else
            {
                speed = 3f;
                SpeedUpImage.fillAmount -= 1.0f / SpeedUpTime * Time.deltaTime;
                WaitSpeedUpTime -= Time.deltaTime;
                
            }
           
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.LogWarning("K");
            if (Time.time >= (LastDash + DashCoolDown))
            {
                isUseDash = true;
                SoundManager.instance.skillAudio();
                ReadyToDash();
                DashImage.fillAmount = 1;
            }
        }
        if (isUseDash)
        {
            DashImage.fillAmount -= 1.0f / DashCoolDown * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.LogWarning("J");
            if (Time.time >= (LastHide + HideCoolDown))
            {
                isUseHide = true;
                SoundManager.instance.skillAudio2();
                ReadyToHide();
                HideImage.fillAmount = 1;
                HideSkillImage.fillAmount = 1;
            }
        }
        if (isUseHide)
        {
            HideSkillImage.fillAmount -= 1.0f / HideCoolDown * Time.deltaTime;
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
            Dash();
        }
        SwitchAnim();
        Hide();
    }
    void Movement() 
    {
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if(movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        rb.velocity = new Vector3(movement.x * speed * Time.fixedDeltaTime * 50, movement.y * speed * Time.fixedDeltaTime * 50, 0.0f);
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        anim.SetFloat("running", movement.magnitude);
        
    }
    void SwitchAnim() 
    {
        if (isHurt)
        {
            anim.SetBool("Hurt", isHurt);
            if (WaitTime < 0.0f)
            {
                isHurt = false;
                anim.SetBool("Hurt", isHurt);
                WaitTime = 0.7f;
            }
            else
            {
                WaitTime -= Time.deltaTime;
            }
        }
        if (isHide)
        {
            if (AnimHidetime > 0)
            {
                anim.SetBool("Hide", true);
                AnimHidetime -= Time.deltaTime;
            }
            else
            {
                anim.SetBool("Hide",false);
            }
        }
        else
        {
            anim.SetBool("Hide", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin") {
            SoundManager.instance.coinAudio();
            Destroy(collision.gameObject);
            Coin = PlayerPrefs.GetInt("Coin");
            Coin += 1;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();
        }

        if (collision.tag == "BigCoin")
        {
            SoundManager.instance.coinAudio();
            Destroy(collision.gameObject);
            Coin = PlayerPrefs.GetInt("Coin");
            Coin += 10;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();
        }
        if (collision.tag == "BlueCoin")
        {
            SoundManager.instance.coinAudio();
            Destroy(collision.gameObject);
            Coin = PlayerPrefs.GetInt("Coin");
            Coin += 5;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();
        }
        if (collision.tag == "Gem")
        {
            SoundManager.instance.gemAudio();
            Destroy(collision.gameObject);
            Gem = PlayerPrefs.GetInt("Gem");
            Gem += 5;
            PlayerPrefs.SetInt("Gem", Gem);
            GemNum.text = Gem.ToString();
            
        }
        if (collision.tag == "Treasure")
        {
            SoundManager.instance.boxAudio();
            Destroy(collision.gameObject);
            Gem = PlayerPrefs.GetInt("Gem");
            Gem += 30;
            PlayerPrefs.SetInt("Gem", Gem);
            GemNum.text = Gem.ToString();
            Coin = PlayerPrefs.GetInt("Coin");
            Coin += 30;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();

        }
        if (collision.tag == "ShadowTreasure")
        {
            SoundManager.instance.boxAudio();
            Destroy(collision.gameObject);
            Gem = PlayerPrefs.GetInt("Gem");
            Gem += 50;
            PlayerPrefs.SetInt("Gem", Gem);
            GemNum.text = Gem.ToString();
            Coin = PlayerPrefs.GetInt("Coin");
            Coin += 50;
            PlayerPrefs.SetInt("Coin", Coin);
            CoinNum.text = Coin.ToString();

        }
        if (collision.tag == "Portal")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            isshop=true;
        }
        if(collision.tag == "Exit")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            isdoor = true;
            isshop = false;
        }
        if (collision.tag == "Dog")
        {
            SoundManager.instance.hitAudio();
            isHurt = true;
        }
        if (collision.gameObject.tag == "Frozen")
        {
            SoundManager.instance.potionAudio();
            FrozenImage.fillAmount = 1;
            WaitFrozenTime = 3f;
            Destroy(collision.gameObject);
            isFrozen = true;
            SetFrozen();
        }
        if (collision.tag == "Trigger")
        {
            SoundManager.instance.triggerAudio();
            Destroy(collision.gameObject);
            // GemAudio.Play();
        }
        if (collision.tag == "Trap")
        {
            SoundManager.instance.hitAudio();
            Destroy(collision.gameObject);
            isHurt = true;
        }
        if (collision.tag == "SpeedUp")
        {
            isSpeedUp = true;
            SpeedUpImage.fillAmount = 1;
            SoundManager.instance.potionAudio();
            Destroy(collision.gameObject);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void ReadyToDash()
    {
        isDash = true;
        DashtimeLeft = Dashtime;
        LastDash = Time.time;
    }
    void Dash()
    {
        if (isDash)
        {
            if(DashtimeLeft > 0)
            {
                float increase = speed * Time.fixedDeltaTime * 50 * Dashspeed;
                rb.velocity = new Vector3(movement.x * increase, movement.y * increase, 0.0f);
                DashtimeLeft -= Time.deltaTime;
                ShadowPool.instance.GetFromPool();
            }
            else
            {
                isDash = false;
            }
        }
    }
    public void buttomDash()
    {
        if (Time.time >= (LastDash + DashCoolDown))
        {
            isUseDash = true;
            SoundManager.instance.skillAudio();
            ReadyToDash();
            DashImage.fillAmount = 1;
        }
    }
    void ReadyToHide()
    {
        isHide = true;
        HidetimeLeft = Hidetime;
        LastHide = Time.time;
    }
    void Hide()
    {
        if (isHide)
        {
            if (HidetimeLeft > 0)
            {
                SetColor(); 
                if (isshop == false)
                {
                    SetHide();
                }
                HidetimeLeft -= Time.deltaTime;
            }
            else
            {

                ResetColor();
                isHide = false;
                if (isshop == false)
                {
                    ReSetHide();
                }
            }
        }
    }
    public void buttomHide()
    {
        if (Time.time >= (LastHide + HideCoolDown))
        {
            isUseHide = true;
            SoundManager.instance.skillAudio2();
            ReadyToHide();
            HideImage.fillAmount = 1;
            HideSkillImage.fillAmount = 1;
        }
    }
    void SetFrozen()
    {
       
        DogAI.setForzen(true);
        DogAI2.setForzen(true);
        DogAI3.setForzen(true);
        DogAI4.setForzen(true);
        DogAI5.setForzen(true);
        DogAI6.setForzen(true);

        CopsAI.setForzen(true);
        CopsAI2.setForzen(true);
        CopsAI3.setForzen(true);
        CopsAI4.setForzen(true);
    }
    void SetHide()
    {

        DogAI.setHide(true);
        DogAI2.setHide(true);
        DogAI3.setHide(true);
        DogAI4.setHide(true);
        DogAI5.setHide(true);
        DogAI6.setHide(true);
        
        CopsAI.setHide(true);
        CopsAI2.setHide(true);
        CopsAI3.setHide(true);
        CopsAI4.setHide(true);
    }
    void ReSetFrozen() 
    {

        DogAI.setForzen(false);
        DogAI2.setForzen(false);
        DogAI3.setForzen(false);
        DogAI4.setForzen(false);
        DogAI5.setForzen(false);
        DogAI6.setForzen(false);

        CopsAI.setForzen(false);
        CopsAI2.setForzen(false);
        CopsAI3.setForzen(false);
        CopsAI4.setForzen(false);
    }
    void ReSetHide()
    {

        DogAI.setHide(false);
        DogAI2.setHide(false);
        DogAI3.setHide(false);
        DogAI4.setHide(false);
        DogAI5.setHide(false);
        DogAI6.setHide(false);

        CopsAI.setHide(false);
        CopsAI2.setHide(false);
        CopsAI3.setHide(false);
        CopsAI4.setHide(false);

    }
    void SetColor()
    {
        sr.color = new Color(1f, 1f, 1f,0.6f);
    }
    void ResetColor()
    {
        sr.color = color;
    }
}
